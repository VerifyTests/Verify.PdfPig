using UglyToad.PdfPig.Content;

class DocumentInformationConverter :
    WriteOnlyJsonConverter<DocumentInformation>
{
    public override void Write(VerifyJsonWriter writer, DocumentInformation value)
    {
        writer.WriteStartObject();
        writer.WriteProperty(value, value.Author, "Author");
        writer.WriteProperty(value, value.Creator, "Creator");
        writer.WriteProperty(value, value.Keywords, "Keywords");
        writer.WriteProperty(value, value.Producer, "Producer");
        writer.WriteProperty(value, value.Title, "Title");
        writer.WriteProperty(value, value.Subject, "Subject");
        writer.WriteProperty(value, value.GetCreatedDateTimeOffset(), "CreationDate");
        writer.WriteProperty(value, value.GetModifiedDateTimeOffset(), "ModifiedDate");
        writer.WriteProperty(value, value.Author, "Author");
        writer.WriteEnd();
    }
}