
using Newtonsoft.Json;

namespace AiFinanceTracker.Server.Functions.Models
{
    public class Transaction
    {
        [JsonProperty("id")]
        public string? Id { get; set; } 
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; } = string.Empty;

        [JsonProperty("items")]
        public List<Item> Items { get; set; } = [];
        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; } = null!;
        [JsonProperty("totalPrice")]
        public double TotalPrice { get; set; }
        [JsonProperty("transactionType")]
        public string TransactionType { get; set; } = string.Empty;
        [JsonProperty("receiptUrl")]
        public string? ReceiptUrl { get; set; }
    }
}
