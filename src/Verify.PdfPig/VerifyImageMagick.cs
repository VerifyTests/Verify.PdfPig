using ImageMagick;

namespace VerifyTests;

public static partial class VerifyImageMagick
{
    /// <summary>
    /// Helper method that calls <see cref="RegisterPdfToPngConverter"/> and
    /// <see cref="RegisterComparers"/>(threshold = .005, metric = ErrorMetric.Fuzz)
    /// </summary>
    public static void Initialize()
    {
        RegisterPdfToPngConverter();
    }

    public static void RegisterPdfToPngConverter() =>
        VerifierSettings.RegisterFileConverter(
            "pdf",
            (stream, context) => Convert(stream, context, MagickFormat.Pdf));

    public static void ImageMagickComparer(this VerifySettings settings, double threshold = .005, ErrorMetric metric = ErrorMetric.Fuzz)
    {
        settings.TryGetExtension(out var extension);
        settings.UseStreamComparer(
            (received, verified, _) => Compare(threshold, metric, GetFormat(extension), received, verified));
    }

    public static SettingsTask ImageMagickComparer(this SettingsTask settings, double threshold = .005, ErrorMetric metric = ErrorMetric.Fuzz)
    {
        settings.TryGetExtension(out var extension);
        var format = GetFormat(extension);
        return settings.UseStreamComparer(
            (received, verified, _) => Compare(threshold, metric, format, received, verified));
    }

    public static void RegisterComparer(double threshold, ErrorMetric metric, string extension, MagickFormat format) =>
        VerifierSettings.RegisterStreamComparer(
            extension,
            (received, verified, _) => Compare(threshold, metric, format, received, verified));

    public static Task<CompareResult> Compare(double threshold, ErrorMetric metric, MagickFormat format, Stream received, Stream verified)
    {
        using var img1 = new MagickImage(received, format);
        using var img2 = new MagickImage(verified, format);
        //https://imagemagick.org/script/command-line-options.php#metric
        var diff = img1.Compare(img2, metric);
        var compare = diff < threshold;
        if (compare)
        {
            return Task.FromResult(CompareResult.Equal);
        }

        var round = Math.Ceiling(diff * 100) / 100;
        return Task.FromResult(CompareResult.NotEqual($@"diff({diff}) < threshold({threshold}).
If this difference is acceptable, use:

 * Globally: VerifyImageMagick.RegisterComparers({round});
 * For one test: Verifier.VerifyFile(""file.jpg"").RegisterComparers({round});"));
    }
}