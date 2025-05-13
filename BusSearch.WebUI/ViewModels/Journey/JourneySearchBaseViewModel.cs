using BusSearch.WebUI.ViewModels.Base;

namespace BusSearch.WebUI.ViewModels.Journey
{
    public class JourneySearchBaseViewModel: JourneyBaseViewModel
    {
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public DateTime DepartureDate { get; set; }

    }
}
