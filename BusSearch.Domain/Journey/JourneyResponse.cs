using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusSearch.Domain.Journey
{
    public class JourneyResponse
    {
        public string Status { get; set; }
        public List<JourneyItem> Data { get; set; }
    }

    public class JourneyItem
    {
        public int Id { get; set; }

        [JsonPropertyName("origin-location")]
        public string OriginLocation { get; set; }

        [JsonPropertyName("destination-location")]
        public string DestinationLocation { get; set; }

        public JourneyDetail Journey { get; set; }
    }

    public class JourneyDetail
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }

        [JsonPropertyName("internet-price")]
        public decimal InternetPrice { get; set; }

    }
}
