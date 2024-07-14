

using Newtonsoft.Json;

namespace AiFinanceTracker.Server.Functions.Models
{
    public class AiTransactionResponse
    {
        [JsonProperty("success")]
        public bool IsSuccess { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; } = null!;
        [JsonProperty("category")]
        public string Category { get; set; } = string.Empty;
        [JsonProperty("items")]
        public List<Item> Items { get; set; } = [];
        [JsonProperty("total_price")]
        public double totalPrice { get; set; }

    }
}
