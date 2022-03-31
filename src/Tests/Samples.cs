[TestFixture]
public class Samples
{
    #region CompareImage

    [Test]
    public Task CompareImage() =>
        VerifyFile("sample.jpg");

    #endregion

    #region VerifyPdf

    [Test]
    public Task VerifyPdf() =>
        VerifyFile("sample.pdf");

    #endregion

    #region VerifyPdfStream

    [Test]
    public Task VerifyPdfStream() =>
        Verify(File.OpenRead("sample.pdf"))
            .UseExtension("pdf");

    #endregion
}