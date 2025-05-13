using System.Text.Json.Serialization;

namespace BusSearch.Infrastructure.Models.ObiletApi.Sessions
{
    public class SessionRequest
    {
        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("connection")]
        public Connection Connection { get; set; }

        [JsonPropertyName("browser")]
        public Browser Browser { get; set; }
    }

    public class Connection
    {
        [JsonPropertyName("ip-address")]
        public string IpAddress { get; set; }

        [JsonPropertyName("port")]
        public string Port { get; set; }
    }
    public class Browser
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
