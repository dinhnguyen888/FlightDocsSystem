namespace FlightDocsSystem.Dtos
{
    public class FileUploadRequest
    {
        public IFormFile file {  get; set; }
        public DocumentCreateDto document { get; set; }

    }
}
