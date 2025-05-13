namespace BusSearch.Application.Models.Dtos
{
    public class JourneySearchDto
    {
        public LocationSummaryDto Origin { get; set; }
        public LocationSummaryDto Destination { get; set; }
        public DateTime DepartureDate { get; set; }

        public List<BusLocationDto> Locations { get; set; }
    }

}
