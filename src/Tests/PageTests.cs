using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;

// Every page of sample.pdf is A4, so each has something to say of itself. These are the pages that
// do not: one with no text has no Text, one of a size that is not a named one has no Info, and one
// that is both is left out of the info altogether.
public class PageTests
{
    [Test]
    public Task NothingToSay()
    {
        var builder = new PdfDocumentBuilder();
        var font = builder.AddStandard14Font(Standard14Font.Helvetica);

        var custom = builder.AddPage(300, 400);
        custom.AddText("A size that is not a named one", 12, new PdfPoint(25, 300), font);

        // No text
        builder.AddPage(PageSize.A5);

        // No text, and a size that is not a named one
        builder.AddPage(300, 400);

        // Only the info is verified: the bytes are whatever this version of PdfPig writes.
        return Verify(new MemoryStream(builder.Build()), "pdf")
            .ExcludeTargets("pdf");
    }
}
