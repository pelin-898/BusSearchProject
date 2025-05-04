using BusSearch.Domain.Interfaces;
using BusSearch.Domain.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusSearch.Domain.Journey
{
    public class JourneyRequest : IDeviceRequest
    {
        [JsonPropertyName("device-session")]
        public DeviceSession DeviceSession { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; } = "tr-TR";

        [JsonPropertyName("data")]
        public JourneyData Data { get; set; }
    }

    public class JourneyData
    {
        [JsonPropertyName("origin-id")]
        public int OriginId { get; set; }

        [JsonPropertyName("destination-id")]
        public int DestinationId { get; set; }

        [JsonPropertyName("departure-date")]
        public string DepartureDate { get; set; }
    }
}
