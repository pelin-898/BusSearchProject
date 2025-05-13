using BusSearch.WebUI.ViewModels.Base;


namespace BusSearch.Application.ViewModels.Journey
{
    public class JourneyViewModel : JourneyBaseViewModel
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public decimal InternetPrice { get; set; }
    }
}
