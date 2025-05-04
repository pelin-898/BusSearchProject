using BusSearch.Domain.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSearch.Domain.Interfaces
{
    public interface IDeviceRequest
    {
        public DeviceSession DeviceSession { get; set; }
        public DateTime Date { get; set; }
        public string Language { get; set; }
    }
}
