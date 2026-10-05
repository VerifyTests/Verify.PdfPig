namespace VerifyTests;

/// <summary>
/// What <c>Initialize</c> used to be given to choose the outputs a pdf is split into. Settings
/// of Verify choose that now, and nothing reads this: it is here so that code still naming it is
/// told what to use in its place.
/// </summary>
[Obsolete(
    "PdfPigOutputs and the outputs argument of Initialize are replaced by settings of Verify: VerifierSettings.PageText(PageTextPlacement.None) to leave out the text. See https://github.com/VerifyTests/Verify.PdfPig#migrating-from-2x",
    true)]
[Flags]
public enum PdfPigOutputs
{
    /// <summary>
    /// No outputs. Only the source document (and info) is emitted.
    /// </summary>
    None = 0,

    /// <summary>
    /// Extract the text of each page into the <c>Text</c> property of each page in the info.
    /// When omitted, text extraction is skipped and the property is not serialized.
    /// </summary>
    Text = 1,

    /// <summary>All outputs.</summary>
    All = Text
}
