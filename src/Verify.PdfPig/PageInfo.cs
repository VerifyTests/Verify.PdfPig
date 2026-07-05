namespace VerifyTests;

class PageInfo
{
    /// <summary>Zero based index of the page within the source document.</summary>
    public int Index;
    public PageSize Size;
    public PageRotationDegrees Rotation;
    public string Text = null!;
}
