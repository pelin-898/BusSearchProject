namespace BusSearch.Application.Models.Dtos
{
    public class JourneySearchDto
    {
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public string OriginName { get; set; }
        public string DestinationName { get; set; }
        public string OriginCityName { get; set; }
        public string DestinationCityName { get; set; }
        public DateTime DepartureDate { get; set; }
        public List<BusLocationDto> Locations { get; set; }
    }

}
