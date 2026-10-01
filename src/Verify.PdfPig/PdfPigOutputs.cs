namespace VerifyTests;

/// <summary>
/// Controls which outputs a pdf is split into during verification.
/// The source pdf target is not controlled by this enum.
/// </summary>
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
