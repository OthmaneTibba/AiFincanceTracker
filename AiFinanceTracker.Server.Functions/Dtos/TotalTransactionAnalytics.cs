

using Newtonsoft.Json;

namespace AiFinanceTracker.Server.Functions.Dtos
{
    public class TotalTransactionAnalytics
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; } = string.Empty;
    }
}
