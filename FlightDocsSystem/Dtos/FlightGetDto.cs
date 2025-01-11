using FlightDocsSystem.Models;

namespace FlightDocsSystem.Dtos
{
    public class FlightGetDto
    {
        public int Id { get; set; }
        public string FlightNo { get; set; }
        public string Route { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string PointOfLoading { get; set; } 
        public string PointOfUnloading { get; set; } 
        public string SignaturePath { get; set; }
        public FlightStatuses FlightStatuses { get; set; }
        public int DocumentCount { get; set; }
        public ICollection<DocumentGetDto> Documents { get; set; }
    }
}
