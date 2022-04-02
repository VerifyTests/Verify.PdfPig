using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace VerifyTests;

public static class VerifyPdfPig
{
    public static void Initialize()
    {
        VerifierSettings.ModifySerialization(_ =>
            _.IgnoreMember<DocumentInformation>(_ =>
                _.DocumentInformationDictionary));
        VerifierSettings.RegisterFileConverter(
            "pdf",
            (stream, context) => Convert(stream, context));
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
                    Text = ContentOrderTextExtractor.GetText(page, true),
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
}