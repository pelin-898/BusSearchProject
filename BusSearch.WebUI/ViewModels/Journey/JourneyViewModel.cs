using BusSearch.WebUI.ViewModels.Base;


namespace BusSearch.Application.ViewModels.Journey
{
    public class JourneyViewModel : JourneyBaseViewModel
    {
        public string OriginTerminal { get; set; }
        public string DestinationTerminal { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public decimal Price { get; set; }
    }
}
