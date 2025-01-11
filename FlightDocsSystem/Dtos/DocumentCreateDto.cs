using FlightDocsSystem.Models;

namespace FlightDocsSystem.Dtos
{
    public class DocumentCreateDto
    {
        public string DocumentName { get; set; }
        public int DocsTypeId { get; set; }
        public string Creator { get; set; }
        public int FlightId { get; set; }
    }
}
