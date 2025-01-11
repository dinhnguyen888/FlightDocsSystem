namespace FlightDocsSystem.Dtos
{
    public class FlightUploadSignatureDto
    {
        public int flightId { get; set; }
        public IFormFile file { get; set; }
    }
}
