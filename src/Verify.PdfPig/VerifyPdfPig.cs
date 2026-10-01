namespace VerifyTests;

public static class VerifyPdfPig
{
    public static bool Initialized { get; private set; }

    static PdfPigOutputs outputs = PdfPigOutputs.All;

    /// <param name="outputs">Which outputs a pdf is split into. Defaults to <see cref="PdfPigOutputs.All"/>.</param>
    public static void Initialize(PdfPigOutputs outputs = PdfPigOutputs.All)
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;
        VerifyPdfPig.outputs = outputs;

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
        var pageContents = new List<PageInfo>();
        PdfInfo info;
        using (var document = PdfDocument.Open(bytes, parsingOptions))
        {
            var numberOfPages = document.NumberOfPages;
            var count = numberOfPages;
            if (context.GetPagesToInclude(out var pagesToInclude))
            {
                count = Math.Min(count, (int) pagesToInclude);
            }

            var includeText = outputs.HasFlag(PdfPigOutputs.Text);
            for (var index = 0; index < count; index++)
            {
                var page = document.GetPage(index + 1);
                pageContents.Add(
                    new()
                    {
                        Index = index,
                        Text = includeText ? TrimWhitespace(ContentOrderTextExtractor.GetText(page, true)) : null,
                        Size = page.Size,
                        Rotation = page.Rotation,
                    });
            }

            info = new()
            {
                Information = document.Information,
                PageCount = numberOfPages,
                Pages = pageContents
            };
        }

        // The pdf snapshot is always the full source document, regardless of PagesToInclude:
        // PagesToInclude only trims the info/text pages, since PdfPig has no in-place page splitter
        // and rebuilding a subset via the writer would re-serialize the whole file.
        List<Target> targets = [];
        // Generating the pdf is expensive, so skip it entirely when the pdf target is excluded.
        if (!context.IsTargetExcluded("pdf"))
        {
            if (context.Normalize())
            {
                // Neutralize the volatile fields for the pdf snapshot only once the document, which
                // reads lazily from the same buffer, has been released.
                bytes = PdfNormalizer.Normalize(bytes);
            }

            targets.Add(
                new("pdf", new MemoryStream(bytes))
                {
                    BypassComparersForSubsequentOnDifference = true
                });
        }

        return new(info, targets);
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