using System.Text;
using DeterministicPdf;

// The Samples entry for SkipPdfNormalization only proves the setting round-trips through a
// verification. These assert the wiring it is actually there for: with the setting the snapshotted
// bytes are the producer's own, and without it they are the normalized ones.
[TestFixture]
public class SkipPdfNormalizationTests
{
    [Test]
    public void SampleIsNotAlreadyNormalized()
    {
        // The premise the two tests below rest on. If sample.pdf were already byte-identical to its
        // normalized form they would both pass while asserting nothing.
        var raw = File.ReadAllBytes("sample.pdf");

        Assert.That(PdfNormalizer.Normalize(raw), Is.Not.EqualTo(raw));
    }

    [Test]
    public Task SkippedSnapshotHoldsTheProducerBytes() =>
        Verify(new MemoryStream(File.ReadAllBytes("sample.pdf")), "pdf")
            .SkipPdfNormalization();

    [Test]
    public Task NormalizedSnapshotHoldsTheNeutralizedBytes() =>
        Verify(new MemoryStream(File.ReadAllBytes("sample.pdf")), "pdf");
}
