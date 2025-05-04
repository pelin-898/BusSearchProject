using BusSearch.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusSearch.Domain.Location
{
    public class BusLocationRequest : IDeviceRequest
    {
        [JsonPropertyName("data")]
        public string? Data { get; set; } = null;

        [JsonPropertyName("device-session")]
        public DeviceSession DeviceSession { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; } = DateTime.Now;

        [JsonPropertyName("language")]
        public string Language { get; set; } = "tr-TR";
    }

    public class DeviceSession
    {
        [JsonPropertyName("session-id")]
        public string SessionId { get; set; }

        [JsonPropertyName("device-id")]
        public string DeviceId { get; set; }
    }
}
