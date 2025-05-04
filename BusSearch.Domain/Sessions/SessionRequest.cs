using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusSearch.Domain.Sessions
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
