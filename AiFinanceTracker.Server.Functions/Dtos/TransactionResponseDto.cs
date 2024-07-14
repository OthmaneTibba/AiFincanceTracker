

using AiFinanceTracker.Server.Functions.Models;
using Newtonsoft.Json;

namespace AiFinanceTracker.Server.Functions.Dtos
{
    public class TransactionResponseDto
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; } = string.Empty;
        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; } = null!;

        [JsonProperty("items")]
        public List<Item> Items { get; set; } = [];

        [JsonProperty("totalPrice")]
        public double TotalPrice { get; set; }
        [JsonProperty("TransactionType")]
        public string TransactionType { get; set; } = string.Empty;
    }
}
