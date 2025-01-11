namespace FlightDocsSystem.Dtos
{
    public class DocumentGetDto
    {
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string DocsTypeName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Creator { get; set; }
        public float LastestVersion { get; set; } 


    }
}
