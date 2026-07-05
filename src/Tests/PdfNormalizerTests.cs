using System.Text;
using UglyToad.PdfPig;

[TestFixture]
public class PdfNormalizerTests
{
    [Test]
    public void NeutralizesVolatileValues()
    {
        var input =
            "/ID [<A1B2C3D4E5F60718> <1122334455667788>] " +
            "/CreationDate(D:20240115093000+05'30') " +
            "/ModDate(D:20240115093000Z) " +
            "<xmp:CreateDate>2024-01-15T09:30:00+05:30</xmp:CreateDate>" +
            "<xmp:ModifyDate>2024-01-15T09:30:00Z</xmp:ModifyDate>" +
            "<xmp:MetadataDate>2024-01-15T09:30:00Z</xmp:MetadataDate>" +
            "<xmpMM:DocumentID>uuid:0f7b2c9a-1234-5678-9abc-def012345678</xmpMM:DocumentID>" +
            "<xmpMM:InstanceID>xmp.iid:1a2b3c4d</xmpMM:InstanceID>";
        var expected =
            "/ID [<0000000000000000> <0000000000000000>] " +
            "/CreationDate(D:00000000000000+00'00') " +
            "/ModDate(D:00000000000000Z) " +
            "<xmp:CreateDate>0000-00-00T00:00:00+00:00</xmp:CreateDate>" +
            "<xmp:ModifyDate>0000-00-00T00:00:00Z</xmp:ModifyDate>" +
            "<xmp:MetadataDate>0000-00-00T00:00:00Z</xmp:MetadataDate>" +
            $"<xmpMM:DocumentID>{new string('0', 41)}</xmpMM:DocumentID>" +
            $"<xmpMM:InstanceID>{new string('0', 16)}</xmpMM:InstanceID>";
        Assert.That(Normalize(input), Is.EqualTo(expected));
    }

    [Test]
    public void CollapsesDifferingValuesToTheSameOutput()
    {
        // The same producer emits a stable structure across runs, so two documents differing only
        // in the volatile digits/hex normalize to identical bytes.
        var a = "/ID [<A1B2C3D4>] /CreationDate(D:20240115093000+05'30')";
        var b = "/ID [<99887766>] /CreationDate(D:19991231235959+11'45')";
        Assert.That(a, Is.Not.EqualTo(b));
        Assert.That(Normalize(a), Is.EqualTo(Normalize(b)));
    }

    [Test]
    public void LeavesLookalikeKeysUntouched()
    {
        // /IDTree is a name-tree key (not the file identifier), /ModDateStamp is a different name,
        // and a self-closing date element has no content: none should be altered.
        var input = "/IDTree [1 2] /ModDateStamp(20240101) <xmp:CreateDate/>2024";
        Assert.That(Normalize(input), Is.EqualTo(input));
    }

    [Test]
    public void NormalizedDocumentStillLoads()
    {
        var data = File.ReadAllBytes("sample.pdf");
        PdfNormalizer.Normalize(data);

        using var document = PdfDocument.Open(data);
        Assert.That(document.NumberOfPages, Is.EqualTo(4));
    }

    static string Normalize(string value)
    {
        var bytes = Encoding.Latin1.GetBytes(value);
        PdfNormalizer.Normalize(bytes);
        return Encoding.Latin1.GetString(bytes);
    }
}
