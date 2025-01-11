namespace FlightDocsSystem.Dtos
{
    public class DocumentUpdateDto
    {
        public string DocumentName { get; set; }
        public string? Note { get; set; }
        public string DocumentPath { get; set; }
        public string? DocsType { get; set; }
    }
}
