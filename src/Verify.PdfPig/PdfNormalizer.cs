/// <summary>
/// Neutralizes the non-deterministic fields of a PDF (the trailer <c>/ID</c>, the document
/// information <c>/CreationDate</c> and <c>/ModDate</c>, and the equivalent XMP metadata dates and
/// identifiers) so that the same source document always produces byte-identical snapshot output.
/// </summary>
/// <remarks>
/// All edits are performed directly on the bytes and are length-preserving: only the mutable
/// characters inside each value are overwritten, so every cross-reference offset stays valid and
/// the file never has to be re-serialized. The scan is plaintext, so a value that has been
/// compressed away (inside an <c>/ObjStm</c> object stream or a flate-compressed XMP packet) no
/// longer appears literally and is therefore left as-is.
/// <para>
/// Targets unencrypted documents. Encrypted PDFs seed their encryption key from the trailer
/// <c>/ID</c>; zeroing it would leave the document undecryptable, so encrypted input should not be
/// passed here.
/// </para>
/// </remarks>
static class PdfNormalizer
{
    enum Fill
    {
        // Zero the ASCII digits only, keeping separators (leaves a readable date).
        Digits,

        // Zero the hexadecimal digits (for hex string <...> values).
        Hex,

        // Zero every non-whitespace byte (for opaque identifiers).
        All
    }

    public static void Normalize(byte[] data)
    {
        // Document information dictionary dates.
        ZeroPdfString(data, "/CreationDate"u8, Fill.Digits);
        ZeroPdfString(data, "/ModDate"u8, Fill.Digits);

        // Trailer / cross-reference-stream file identifier: /ID [<...> <...>].
        ZeroFileId(data);

        // XMP metadata dates (uncompressed metadata streams only).
        ZeroXmpElement(data, "<xmp:CreateDate"u8, Fill.Digits);
        ZeroXmpElement(data, "<xmp:ModifyDate"u8, Fill.Digits);
        ZeroXmpElement(data, "<xmp:MetadataDate"u8, Fill.Digits);

        // XMP per-generation identifiers.
        ZeroXmpElement(data, "<xmpMM:DocumentID"u8, Fill.All);
        ZeroXmpElement(data, "<xmpMM:InstanceID"u8, Fill.All);
        ZeroXmpElement(data, "<xmpMM:OriginalDocumentID"u8, Fill.All);
    }

    // Finds a name key, then overwrites the string value that follows it. The value may be a
    // literal string "(...)" or a hex string "<...>".
    static void ZeroPdfString(byte[] data, ReadOnlySpan<byte> key, Fill fill)
    {
        var pos = 0;
        while (true)
        {
            var hit = data.AsSpan(pos).IndexOf(key);
            if (hit < 0)
            {
                return;
            }

            var i = pos + hit + key.Length;
            pos = i;

            i = SkipWhitespace(data, i);
            if (i >= data.Length)
            {
                return;
            }

            if (data[i] == (byte) '(')
            {
                var start = i + 1;
                var end = FindLiteralEnd(data, start);
                Overwrite(data, start, end, fill);
                pos = end;
            }
            else if (data[i] == (byte) '<' && (i + 1 >= data.Length || data[i + 1] != (byte) '<'))
            {
                var start = i + 1;
                var end = FindByte(data, start, (byte) '>');
                Overwrite(data, start, end, Fill.Hex);
                pos = end;
            }
        }
    }

    // Finds "/ID" followed by an array and zeroes each string element. Anything not shaped like the
    // identifier array (for example the "/IDTree" name-tree key) is skipped.
    static void ZeroFileId(byte[] data)
    {
        var key = "/ID"u8;
        var pos = 0;
        while (true)
        {
            var hit = data.AsSpan(pos).IndexOf(key);
            if (hit < 0)
            {
                return;
            }

            var i = pos + hit + key.Length;
            pos = i;

            i = SkipWhitespace(data, i);
            if (i >= data.Length || data[i] != (byte) '[')
            {
                continue;
            }

            i++;
            while (i < data.Length && data[i] != (byte) ']')
            {
                if (data[i] == (byte) '<')
                {
                    var start = i + 1;
                    i = FindByte(data, start, (byte) '>');
                    Overwrite(data, start, i, Fill.Hex);
                    i++;
                }
                else if (data[i] == (byte) '(')
                {
                    var start = i + 1;
                    i = FindLiteralEnd(data, start);
                    Overwrite(data, start, i, Fill.All);
                    i++;
                }
                else
                {
                    i++;
                }
            }

            pos = i;
        }
    }

    // Finds an XMP element by its opening tag and zeroes the text content up to the next '<'.
    static void ZeroXmpElement(byte[] data, ReadOnlySpan<byte> openTag, Fill fill)
    {
        var pos = 0;
        while (true)
        {
            var hit = data.AsSpan(pos).IndexOf(openTag);
            if (hit < 0)
            {
                return;
            }

            var i = pos + hit + openTag.Length;
            pos = i;

            // Reject a longer element name that merely shares this prefix.
            if (i < data.Length && data[i] != (byte) '>' && data[i] != (byte) '/' && !IsWhitespace(data[i]))
            {
                continue;
            }

            // Skip the remainder of the opening tag, remembering the last significant byte so a
            // self-closing "<tag/>" can be detected.
            var lastSignificant = (byte) 0;
            while (i < data.Length && data[i] != (byte) '>')
            {
                if (!IsWhitespace(data[i]))
                {
                    lastSignificant = data[i];
                }

                i++;
            }

            if (i >= data.Length)
            {
                return;
            }

            i++;
            if (lastSignificant == (byte) '/')
            {
                continue;
            }

            var start = i;
            var end = FindByte(data, start, (byte) '<');
            Overwrite(data, start, end, fill);
            pos = end;
        }
    }

    static void Overwrite(byte[] data, int start, int end, Fill fill)
    {
        for (var i = start; i < end; i++)
        {
            var c = data[i];
            var replace = fill switch
            {
                Fill.Digits => IsDigit(c),
                Fill.Hex => IsHexDigit(c),
                _ => !IsWhitespace(c)
            };
            if (replace)
            {
                data[i] = (byte) '0';
            }
        }
    }

    // Returns the index of the ')' that closes the literal string starting at 'start', honoring
    // backslash escapes and balanced parentheses, or the end of the buffer if unterminated.
    static int FindLiteralEnd(byte[] data, int start)
    {
        var depth = 1;
        var i = start;
        while (i < data.Length)
        {
            var c = data[i];
            if (c == (byte) '\\')
            {
                i += 2;
                continue;
            }

            if (c == (byte) '(')
            {
                depth++;
            }
            else if (c == (byte) ')')
            {
                depth--;
                if (depth == 0)
                {
                    return i;
                }
            }

            i++;
        }

        return data.Length;
    }

    static int FindByte(byte[] data, int start, byte target)
    {
        var i = start;
        while (i < data.Length && data[i] != target)
        {
            i++;
        }

        return i;
    }

    static int SkipWhitespace(byte[] data, int i)
    {
        while (i < data.Length && IsWhitespace(data[i]))
        {
            i++;
        }

        return i;
    }

    static bool IsDigit(byte b) =>
        b is >= (byte) '0' and <= (byte) '9';

    static bool IsHexDigit(byte b) =>
        b is >= (byte) '0' and <= (byte) '9' or >= (byte) 'a' and <= (byte) 'f' or >= (byte) 'A' and <= (byte) 'F';

    static bool IsWhitespace(byte b) =>
        b is (byte) ' ' or (byte) '\t' or (byte) '\r' or (byte) '\n' or (byte) '\f' or 0;
}
