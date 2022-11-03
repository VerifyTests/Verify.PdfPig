using UglyToad.PdfPig;

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

    /// <summary>
    /// Issue address:
    /// https://github.com/VerifyTests/Verify.PdfPig/issues/1
    /// </summary>
    /// <returns></returns>
    [Test]
    public Task PdfWords_Issue_1()
    {
        using (PdfDocument document = PdfDocument.Open(File.OpenRead("sample.pdf")))
        {
            foreach (var page in document.GetPages())
            {
                var text = page.Text;
                Assert.True(!text.Contains("commodo a.Etiam vehicula"));// it must be "commodo a. Etiam vehicula" a space or \r\n between pragraphs.
            }
        }

        return Task.CompletedTask;
    }
}