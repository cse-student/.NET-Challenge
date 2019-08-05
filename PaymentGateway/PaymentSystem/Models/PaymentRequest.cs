using Newtonsoft.Json;

namespace PaymentSystem.Gateway.Models
{
  public class PaymentRequest
  {
    [JsonProperty("card_number")]
    public string CardNumber { get; set; }
    [JsonProperty("expiry_date")]
    public string ExpiryDate { get; set; }
    [JsonProperty("amount")]
    public double Amount { get; set; }
    [JsonProperty("currency")]
    public string Currency { get; set; }
    [JsonProperty("cvv")]
    public int Cvv { get; set; }
  }
}
