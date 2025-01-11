
using FlightDocsSystem.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace FlightDocsSystem.Dtos
{
    public class FlightUpdateDto
     {

        public string FlightNo { get; set; }
        public string Route { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string PointOfLoading { get; set; }
        public string PointOfUnloading { get; set; }
        [SwaggerSchema("Flight status (Start or End)")]
        public FlightStatuses FlightStatus { get; set; }
       
    }
}
