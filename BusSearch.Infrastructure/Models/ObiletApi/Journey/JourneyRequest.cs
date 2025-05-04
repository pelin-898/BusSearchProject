using BusSearch.Application.Constants;
using BusSearch.Infrastructure.Models.ObiletApi.Location;
using System.Text.Json.Serialization;

namespace BusSearch.Infrastructure.Models.ObiletApi.Journey
{
    public class JourneyRequest : IDeviceRequest
    {
        [JsonPropertyName("device-session")]
        public DeviceSession DeviceSession { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; } = Languages.DefaultLanguage;

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
