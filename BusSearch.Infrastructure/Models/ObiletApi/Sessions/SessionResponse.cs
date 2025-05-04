using System.Text.Json.Serialization;


namespace BusSearch.Infrastructure.Models.ObiletApi.Sessions
{
    public class SessionResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public SessionData Data { get; set; }

    }

    public class SessionData
    {
        [JsonPropertyName("session-id")]
        public string SessionId { get; set; }

        [JsonPropertyName("device-id")]
        public string DeviceId { get; set; }

    }
}
