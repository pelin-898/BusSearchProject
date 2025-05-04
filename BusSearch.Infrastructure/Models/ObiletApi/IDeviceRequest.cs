using BusSearch.Infrastructure.Models.ObiletApi.Location;

namespace BusSearch.Infrastructure.Models.ObiletApi
{
    public interface IDeviceRequest
    {
        public DeviceSession DeviceSession { get; set; }
        public DateTime Date { get; set; }
        public string Language { get; set; }
    }
}
