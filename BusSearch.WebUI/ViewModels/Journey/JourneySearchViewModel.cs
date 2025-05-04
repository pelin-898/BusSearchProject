using BusSearch.Application.Models.Dtos;
using BusSearch.WebUI.ViewModels.Base;

namespace BusSearch.WebUI.ViewModels.Journey
{
    public class JourneySearchViewModel : JourneyBaseViewModel 
    {
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public string OriginCityName { get; set; }
        public string DestinationCityName { get; set; }
        public List<BusLocationDto> Locations { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
