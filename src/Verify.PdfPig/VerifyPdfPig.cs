using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace VerifyTests;

public static class VerifyPdfPig
{
    public static void Initialize()
    {
        VerifierSettings
            .AddExtraSettings(
                _ => _.Converters.Add(new DocumentInformationConverter()));
        VerifierSettings.RegisterFileConverter("pdf", Convert);
    }

    static ConversionResult Convert(Stream stream, IReadOnlyDictionary<string, object> context)
    {
        var pageContents = new List<PageInfo>();
        var parsingOptions = context.PdfPigParsingOptions();
        using var document = PdfDocument.Open(stream, parsingOptions);
        var count = document.NumberOfPages;
        if (context.GetPagesToInclude(out var pagesToInclude))
        {
            count = Math.Min(count, (int) pagesToInclude);
        }

        for (var index = 0; index < count; index++)
        {
            var page = document.GetPage(index + 1);
            pageContents.Add(
                new()
                {
                    Text = TrimWhitespace(ContentOrderTextExtractor.GetText(page, true)),
                    Size = page.Size,
                    Rotation = page.Rotation,
                });
        }

        return new(
            new PdfInfo
            {
                Information = document.Information,
                Pages = pageContents
            },
            Enumerable.Empty<Target>());
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