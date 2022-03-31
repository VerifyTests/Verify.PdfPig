[TestFixture]
public class Tests
{
    [Test]
    public Task FailingCompare() =>
        ThrowsTask(async () =>
            {
                await VerifyFile("sample.jpg")
                    .DisableDiff()
                    .UseMethodName("FailingCompareInner")
                    .ImageMagickComparer(.0001);
            })
            .IgnoreStackTrack()
            .ScrubLinesContaining("clipboard", "DiffEngineTray");
}