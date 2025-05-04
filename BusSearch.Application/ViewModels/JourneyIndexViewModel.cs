using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusSearch.Application.ViewModels.Base;

namespace BusSearch.Application.ViewModels
{
    public class JourneyIndexViewModel : JourneyBaseViewModel
    {
        public List<JourneyViewModel> Journeys { get; set; }
        public string DepartureDate { get; set; }
    }
}
