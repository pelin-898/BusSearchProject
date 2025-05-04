using BusSearch.Application.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSearch.Application.ViewModels
{
    public class JourneyViewModel : JourneyBaseViewModel
    {
        public string OriginTerminal { get; set; }
        public string DestinationTerminal { get; set; }
        public string DepartureTime{ get; set; }
        public string ArrivalTime { get; set; }
        public decimal Price { get; set; }
    }
}
