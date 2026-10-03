public class Samples
{
    [Test]
    public Task NoText() =>
        VerifyFile(ProjectFiles.sample_pdf.Path);
}
