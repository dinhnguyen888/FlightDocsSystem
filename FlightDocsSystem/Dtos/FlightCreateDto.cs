using FlightDocsSystem.Models;

namespace FlightDocsSystem.Dtos
{
    public class FlightCreateDto
    {
        public string FlightNo { get; set; }
        public string Route { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string PointOfLoading { get; set; } 
        public string PointOfUnloading { get; set; } // Diem den
        public FlightStatuses FlightStatus { get; set; }
      
    }
}
