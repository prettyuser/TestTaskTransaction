using Newtonsoft.Json;

namespace TransactionTestCode.Models;

public class Transaction
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("transactionDate")]
    public DateTime TransactionDate { get; set; }
    
    [JsonProperty("amount")]
    public decimal Amount { get; set; }
}
