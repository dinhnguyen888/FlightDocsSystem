namespace FlightDocsSystem.Dtos
{
    public class DocumentUploadDto
    {
        public IFormFile file { get; set; }
        public DocumentCreateDto documentCreateDto { get; set; }
    }
}
