public class Samples
{
    #region VerifyPdf

    [Test]
    public Task VerifyPdf() =>
        VerifyFile("sample.pdf")
            .PagesToInclude(2);

    #endregion

    #region VerifyPdfStream

    [Test]
    public Task VerifyPdfStream() =>
        Verify(File.OpenRead("sample.pdf"), "pdf");

    #endregion

    #region SkipPdfNormalization

    [Test]
    public Task SkipPdfNormalization() =>
        VerifyFile("sample.pdf")
            .SkipPdfNormalization();

    #endregion

    #region ExcludePdf

    [Test]
    public Task ExcludePdf() =>
        VerifyFile("sample.pdf")
            .ExcludeTargets("pdf");

    #endregion

    #region PageTextPerPage

    [Test]
    public Task PageTextPerPage() =>
        VerifyFile("sample.pdf")
            .PageText(PageTextPlacement.PerPage);

    #endregion

    // PagesToInclude limits the text files as it does the text in the info file: there is one for
    // the page that is asked for and none for the others, and it keeps the number of its page.
    [Test]
    public Task PageTextPerPageOfIncludedPages() =>
        VerifyFile("sample.pdf")
            .PageText(PageTextPlacement.PerPage)
            .PagesToInclude(_ => _ == 2)
            .ExcludeTargets("pdf");
}