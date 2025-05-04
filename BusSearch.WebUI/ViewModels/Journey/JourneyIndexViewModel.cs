using BusSearch.Application.ViewModels.Journey;
using BusSearch.WebUI.ViewModels.Base;


namespace BusSearch.WebUI.ViewModels.Journey
{
    public class JourneyIndexViewModel : JourneyBaseViewModel
    {
        public List<JourneyViewModel> Journeys { get; set; }
        public string DepartureDate { get; set; }
    }
}
