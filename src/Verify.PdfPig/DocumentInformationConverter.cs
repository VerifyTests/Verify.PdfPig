using UglyToad.PdfPig.Content;

class DocumentInformationConverter :
    WriteOnlyJsonConverter<DocumentInformation>
{
    public override void Write(VerifyJsonWriter writer, DocumentInformation value)
    {
        writer.WriteStartObject();
        writer.WriteMember(value, value.Author, "Author");
        writer.WriteMember(value, value.Creator, "Creator");
        writer.WriteMember(value, value.Keywords, "Keywords");
        writer.WriteMember(value, value.Producer, "Producer");
        writer.WriteMember(value, value.Title, "Title");
        writer.WriteMember(value, value.Subject, "Subject");
        writer.WriteMember(value, value.GetCreatedDateTimeOffset(), "CreationDate");
        writer.WriteMember(value, value.GetModifiedDateTimeOffset(), "ModifiedDate");
        writer.WriteEnd();
    }
}