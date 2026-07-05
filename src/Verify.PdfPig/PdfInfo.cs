namespace VerifyTests;

class PdfInfo
{
    public DocumentInformation Information = null!;

    /// <summary>
    /// Total number of pages in the source document. This is the full count regardless of any
    /// <c>PagesToInclude</c> filter; the pages actually rendered are listed in <see cref="Pages"/>,
    /// each carrying its <see cref="PageInfo.Index"/> within the document.
    /// </summary>
    public int PageCount;

    public List<PageInfo> Pages = null!;
}
