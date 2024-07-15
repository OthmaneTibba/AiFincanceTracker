

using Newtonsoft.Json;

namespace AiFinanceTracker.Server.Functions.Dtos
{
    public class CategoryItemExpenseAnalytics
    {
        [JsonProperty("category")]
        public string Category { get; set; } = string.Empty;
        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}
