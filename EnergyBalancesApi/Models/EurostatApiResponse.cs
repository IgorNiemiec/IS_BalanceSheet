using System.Text.Json.Serialization;

namespace EnergyBalancesApi.Models
{
    public class EurostatApiResponse
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("updated")]
        public string UpdatedRaw { get; set; }

        [JsonIgnore]
        public DateTime Updated => DateTime.TryParse(UpdatedRaw, out var date) ? date : DateTime.MinValue;

        [JsonPropertyName("value")]
        public Dictionary<string, double> Value { get; set; }

        
        [JsonPropertyName("dimension")]
        public Dictionary<string, DimensionInfo> Dimension { get; set; }
    }

    public class DimensionInfo
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("category")]
        public CategoryInfo Category { get; set; }
    }

    public class CategoryInfo
    {
        [JsonPropertyName("index")]
        public Dictionary<string, int> Index { get; set; }

        [JsonPropertyName("label")]
        public Dictionary<string, string> Label { get; set; }
    }
}
