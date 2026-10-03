using System.Text;
using DeterministicPdf;

// The Samples entry for SkipPdfNormalization only proves the setting round-trips through a
// verification. These assert the wiring it is actually there for: with the setting the snapshotted
// bytes are the producer's own, and without it they are the normalized ones.
public class SkipPdfNormalizationTests
{
    [Test]
    public async Task SampleIsNotAlreadyNormalized()
    {
        // The premise the two tests below rest on. If sample.pdf were already byte-identical to its
        // normalized form they would both pass while asserting nothing.
        var raw = File.ReadAllBytes(ProjectFiles.sample_pdf);

        await Assert.That(PdfNormalizer.Normalize(raw).SequenceEqual(raw)).IsFalse();
    }

    [Test]
    public Task SkippedSnapshotHoldsTheProducerBytes() =>
        Verify(new MemoryStream(File.ReadAllBytes(ProjectFiles.sample_pdf)), "pdf")
            .SkipPdfNormalization();

    [Test]
    public Task NormalizedSnapshotHoldsTheNeutralizedBytes() =>
        Verify(new MemoryStream(File.ReadAllBytes(ProjectFiles.sample_pdf)), "pdf");
}
