using System.Text.Json.Serialization;


namespace BusSearch.Infrastructure.Models.ObiletApi.Location
{
    public class BusLocationResponse
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
