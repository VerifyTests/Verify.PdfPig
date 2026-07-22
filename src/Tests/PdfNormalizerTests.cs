using System.Text;
using DeterministicPdf;
using UglyToad.PdfPig;

// The neutralizing algorithm itself is owned and tested by the DeterministicPdf package. What is
// worth asserting here is the wiring: that this package applies it, and that a normalized document
// is still loadable by PdfPig.
[TestFixture]
public class PdfNormalizerTests
{
    [Test]
    public void NormalizedDocumentStillLoads()
    {
        var data = PdfNormalizer.Normalize(File.ReadAllBytes("sample.pdf"));

        using var document = PdfDocument.Open(data);
        Assert.That(document.NumberOfPages, Is.EqualTo(4));
    }

    [Test]
    public void NeutralizesVolatileValues()
    {
        var data = PdfNormalizer.Normalize(File.ReadAllBytes("sample.pdf"));

        var text = Encoding.Latin1.GetString(data);
        Assert.Multiple(() =>
        {
            Assert.That(text, Does.Not.Match(@"/CreationDate\s*\(D:[1-9]"));
            Assert.That(text, Does.Not.Match(@"/ModDate\s*\(D:[1-9]"));
        });
    }

    [Test]
    public void IsIdempotent()
    {
        // A second pass has nothing left to change: normalizing already-normalized bytes is a no-op.
        var once = PdfNormalizer.Normalize(File.ReadAllBytes("sample.pdf"));
        var twice = PdfNormalizer.Normalize(once);

        Assert.That(twice, Is.EqualTo(once));
    }
}
