using BusSearch.Application.ViewModels.Base;
using BusSearch.Domain.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSearch.Application.ViewModels
{
    public class JourneySearchViewModel : JourneyBaseViewModel
    {
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public string OriginCityName { get; set; }
        public string DestinationCityName { get; set; }
        public List<BusLocation> Locations { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
