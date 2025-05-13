namespace BusSearch.Application.Models.Dtos
{
    public class JourneyDto
    {
        public string OriginLocation { get; set; }

        public string DestinationLocation { get; set; }

        public JourneyDetailDto Journey { get; set; }
    }

    public class JourneyDetailDto
    {
        public string Origin { get; set; }
        public string Destination { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        public decimal InternetPrice { get; set; }
    }
}