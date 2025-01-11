namespace FlightDocsSystem.Models
{
    public class DocsType
    {
        public int Id { get; set; }
        public string DocsTypeName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Creator { get; set; }
        public string Note { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
