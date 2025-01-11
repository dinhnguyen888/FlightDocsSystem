namespace FlightDocsSystem.Dtos
{
    public class DocumentUpdateFileDto
    {
        public DocumentUpdateDto documentUpdateDto {  get; set; }
        public IFormFile? file { get; set; }
    }
}
