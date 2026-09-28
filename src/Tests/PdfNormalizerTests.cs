using System.Text;
using System.Text.RegularExpressions;
using DeterministicPdf;
using UglyToad.PdfPig;

// The neutralizing algorithm itself is owned and tested by the DeterministicPdf package. What is
// worth asserting here is the wiring: that this package applies it, and that a normalized document
// is still loadable by PdfPig.
public class PdfNormalizerTests
{
    [Test]
    public async Task NormalizedDocumentStillLoads()
    {
        var data = PdfNormalizer.Normalize(File.ReadAllBytes("sample.pdf"));

        using var document = PdfDocument.Open(data);
        await Assert.That(document.NumberOfPages).IsEqualTo(4);
    }

    [Test]
    public async Task NeutralizesVolatileValues()
    {
        var data = PdfNormalizer.Normalize(File.ReadAllBytes("sample.pdf"));

        var text = Encoding.Latin1.GetString(data);
        using (Assert.Multiple())
        {
            await Assert.That(Regex.IsMatch(text, @"/CreationDate\s*\(D:[1-9]")).IsFalse();
            await Assert.That(Regex.IsMatch(text, @"/ModDate\s*\(D:[1-9]")).IsFalse();
        }
    }

    [Test]
    public async Task IsIdempotent()
    {
        // A second pass has nothing left to change: normalizing already-normalized bytes is a no-op.
        var once = PdfNormalizer.Normalize(File.ReadAllBytes("sample.pdf"));
        var twice = PdfNormalizer.Normalize(once);

        await Assert.That(twice.SequenceEqual(once)).IsTrue();
    }
}
