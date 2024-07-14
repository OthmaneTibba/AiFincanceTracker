

using Newtonsoft.Json;

namespace AiFinanceTracker.Server.Functions.Models
{
    public class Merchant
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
    }
}
