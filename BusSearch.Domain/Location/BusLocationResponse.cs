using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusSearch.Domain.Location
{
    public class BusLocationResponse
    {
        public string Status { get; set; }
        public List<BusLocation> Data { get; set; }
    }

    public class BusLocation
    {
        public int Id { get; set; }

        [JsonPropertyName("parent-id")]
        public int? ParentId { get; set; }

        [JsonPropertyName("city-name")]
        public string CityName { get; set; }
        public string Name { get; set; }
        public int? Rank { get; set; }

    }
}
