namespace FlightDocsSystem.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNo { get; set; }
        public string Route { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate {  get; set; }
        public string PointOfLoading { get; set; } //Diem khoi hanh
        public string PointOfUnloading { get; set; } //Diem den
        public FlightStatuses FlightStatus { get; set; }
        public string? SignaturePath { get; set; } //chu ky (danh cho phi cong )
        public ICollection<Document> Documents { get; set; }
        
    }

    public enum FlightStatuses
    {

        Start,
        End
    }

}
