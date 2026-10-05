namespace VerifyTests;

public static class VerifyPdfPig
{
    public static bool Initialized { get; private set; }

    public static void Initialize()
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();
        VerifierSettings
            .AddExtraSettings(
                _ => _.Converters.Add(new DocumentInformationConverter()));
        VerifierSettings.RegisterStreamConverter("pdf", Convert);
    }

    static ConversionResult Convert(string? name, Stream stream, IReadOnlyDictionary<string, object> context)
    {
        var bytes = ToBytes(stream);
        var parsingOptions = context.PdfPigParsingOptions();

        // Places the text of each page, and says which pages the verification wants and whether it
        // wants their text, so text that is not wanted is not extracted.
        var conversion = new PagedConversion(context);
        var includeText = conversion.IncludeText;
        using (var document = PdfDocument.Open(bytes, parsingOptions))
        {
            conversion.Info = document.Information;
            foreach (var number in conversion.Pages(document.NumberOfPages))
            {
                var page = document.GetPage(number);

                string? text = null;
                if (includeText)
                {
                    text = ReadText(page);
                }

                conversion.AddPage(number, text: text, info: ReadInfo(page));
            }
        }

        // Generating the pdf is expensive, so skip it entirely when the pdf target is excluded.
        if (!context.IsTargetExcluded("pdf"))
        {
            if (context.Normalize())
            {
                // Neutralize the volatile fields for the pdf snapshot only once the document, which
                // reads lazily from the same buffer, has been released.
                bytes = PdfNormalizer.Normalize(bytes);
            }

            conversion.Source(new("pdf", new MemoryStream(bytes)));
        }

        return conversion.Build();
    }

    static byte[] ToBytes(Stream stream)
    {
        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using var buffer = new MemoryStream();
        stream.CopyTo(buffer);
        return buffer.ToArray();
    }

    // Null for a page there is nothing to say of: a size that is not a named one, and no rotation.
    // Both are then defaults, which the info leaves out, and the page would have an empty Info.
    static PageInfo? ReadInfo(Page page)
    {
        if (page.Size == PageSize.Custom &&
            page.Rotation.Value == 0)
        {
            return null;
        }

        return new()
        {
            Size = page.Size,
            Rotation = page.Rotation
        };
    }

    // Null for a page with no text, so that nothing is verified for it. An empty text would be an
    // empty Text in the info, or under PageTextPlacement.PerPage a file with nothing in it.
    static string? ReadText(Page page)
    {
        var text = TrimWhitespace(ContentOrderTextExtractor.GetText(page, true));
        if (text.Length == 0)
        {
            return null;
        }

        return text;
    }

    static string TrimWhitespace(string text)
    {
        var builder = new StringBuilder(text.Length);
        using var reader = new StringReader(text);

        while (reader.ReadLine() is { } line)
        {
            builder.AppendLine(line.Trim());
        }

        return builder.ToString();
    }
}