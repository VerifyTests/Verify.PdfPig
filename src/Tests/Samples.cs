[TestFixture]
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
}