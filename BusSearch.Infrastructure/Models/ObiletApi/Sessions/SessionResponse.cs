using System.Text.Json.Serialization;

namespace BusSearch.Infrastructure.Models.ObiletApi.Sessions
{
    public class SessionResponse
    {

        [JsonPropertyName("session-id")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("device-id")]
        public string DeviceId { get; set; } = string.Empty;
    }
}
