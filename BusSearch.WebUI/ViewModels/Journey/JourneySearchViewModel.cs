using BusSearch.Application.Models.Dtos;

namespace BusSearch.WebUI.ViewModels.Journey
{
    public class JourneySearchViewModel : JourneySearchBaseViewModel 
    {
        public string OriginCityName { get; set; }
        public string DestinationCityName { get; set; }
        public List<BusLocationDto> Locations { get; set; }
    }
}
