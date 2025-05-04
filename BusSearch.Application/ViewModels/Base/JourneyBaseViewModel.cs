using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSearch.Application.ViewModels.Base
{
    public abstract class JourneyBaseViewModel
    {
        public string OriginName { get; set; }
        public string DestinationName { get; set; }
    }
}
