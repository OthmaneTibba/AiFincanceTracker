

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AiFinanceTracker.Server.Functions.Models
{

    public class Item
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("category")]
        public string Category { get; set; } = string.Empty;
        [JsonProperty("price")]
        public double Price { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
