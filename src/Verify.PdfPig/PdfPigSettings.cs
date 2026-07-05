namespace VerifyTests;

public static class PdfPigSettings
{
    /// <summary>
    /// Limits the number of pages included in the info/text snapshot to <paramref name="count"/>.
    /// The <c>pdf</c> snapshot is unaffected and always contains the full source document.
    /// </summary>
    public static void PagesToInclude(this VerifySettings settings, int count) =>
        settings.Context["PdfPig.PagesToInclude"] = count;

    /// <inheritdoc cref="PagesToInclude(VerifySettings, int)"/>
    public static SettingsTask PagesToInclude(this SettingsTask settings, int count)
    {
        settings.CurrentSettings.PagesToInclude(count);
        return settings;
    }

    internal static bool GetPagesToInclude(this IReadOnlyDictionary<string, object> context, [NotNullWhen(true)] out int? pages)
    {
        if (context.TryGetValue("PdfPig.PagesToInclude", out var value))
        {
            pages = (int) value;
            return true;
        }

        pages = null;
        return false;
    }

    public static void PdfPigParsingOptions(this VerifySettings settings, ParsingOptions options) =>
        settings.Context["PdfPig.ParsingOptions"] = options;

    public static SettingsTask PdfPigParsingOptions(this SettingsTask settings, ParsingOptions options)
    {
        settings.CurrentSettings.PdfPigParsingOptions(options);
        return settings;
    }

    internal static ParsingOptions PdfPigParsingOptions(this IReadOnlyDictionary<string, object> context)
    {
        if (context.TryGetValue("PdfPig.ParsingOptions", out var value))
        {
            return (ParsingOptions) value;
        }

        return new();
    }
}