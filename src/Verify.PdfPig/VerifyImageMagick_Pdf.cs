using ImageMagick;
using UglyToad.PdfPig;

namespace VerifyTests;

public static partial class VerifyPdfPig
{
    static ConversionResult Convert(Stream stream, IReadOnlyDictionary<string, object> context, MagickFormat magickFormat)
    {
        var streams = new List<Stream>();
        var parsingOptions = context.PdfPigParsingOptions();
        var document = PdfDocument.Open(stream,parsingOptions);
        document.
        magickSettings.Format = magickFormat;
        using var images = new MagickImageCollection();
        images.Read(stream, magickSettings);
        var count = images.Count;
        if (context.GetPagesToInclude(out var pagesToInclude))
        {
            count = Math.Min(count, (int) pagesToInclude);
        }

        for (var index = 0; index < count; index++)
        {
            var page = document.GetPage(index);
            UglyToad.PdfPig.Rendering.IPageImageRenderer
            page.
            var image = images[index];
            var memoryStream = new MemoryStream();
            image.Write(memoryStream, MagickFormat.Png);
            page.
            streams.Add(memoryStream);
        }

        return new(null, streams.Select(x => new Target("png", x)));
    }
}