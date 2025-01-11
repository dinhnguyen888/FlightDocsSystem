using System.Text.Json.Serialization;

namespace FlightDocsSystem.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public int DocsTypeId { get; set; }
        [JsonIgnore]
        public DocsType DocsType { get; set; }
        public DateTime CreateDate { get; set; }
        public string Creator {  get; set; }
        public double LastestVersion { get; set; } = 1;
        public string DocumentPath { get; set; }
        public int FlightId { get; set; }
        [JsonIgnore]
        public Flight Flight { get; set; }
        

    }
}
