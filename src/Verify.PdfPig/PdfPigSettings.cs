namespace VerifyTests;

public static class PdfPigSettings
{
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

    /// <summary>
    /// Snapshots the pdf bytes exactly as produced, skipping the normalization that neutralizes the
    /// trailer <c>/ID</c>, the <c>/CreationDate</c> and <c>/ModDate</c>, and the XMP dates and
    /// identifiers. Use it when the producer already emits byte-deterministic documents, since
    /// normalizing them again copies the whole buffer, rescans it, and — when the XMP packet is
    /// canonicalized — rebuilds it and repairs the cross-reference table, all to change nothing.
    /// </summary>
    /// <remarks>
    /// Only skip this when the producer is genuinely deterministic. Without it a freshly generated
    /// pdf carries a wall-clock <c>/CreationDate</c> and a fresh <c>/ID</c>, so the snapshot differs
    /// on every run.
    /// <para>
    /// The XMP canonicalization is worth calling out because it is the pass that changes bytes for
    /// an already-deterministic producer: it collapses the packet's whitespace, so enabling or
    /// disabling this setting on an existing suite shifts the stored <c>.verified.pdf</c> even
    /// though nothing about the document changed. Expect to re-accept those snapshots once.
    /// </para>
    /// </remarks>
    public static void SkipPdfNormalization(this VerifySettings settings) =>
        settings.Context["PdfPig.SkipNormalization"] = true;

    /// <inheritdoc cref="SkipPdfNormalization(VerifySettings)"/>
    public static SettingsTask SkipPdfNormalization(this SettingsTask settings)
    {
        settings.CurrentSettings.SkipPdfNormalization();
        return settings;
    }

    internal static bool Normalize(this IReadOnlyDictionary<string, object> context) =>
        !context.TryGetValue("PdfPig.SkipNormalization", out var value) ||
        value is not true;
}
