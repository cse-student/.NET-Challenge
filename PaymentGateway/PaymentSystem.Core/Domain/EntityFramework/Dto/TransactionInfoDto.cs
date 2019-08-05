using System;
using Newtonsoft.Json;

namespace PaymentSystem.Core.Domain.EntityFramework.Dto
{
  public class TransactionInfoDto
  {
    public string MerchantId { get; set; }
    public string TransactionId { get; set; }
    public bool Success { get; set; }
    public PaymentInfoDto PaymentInfo { get; set; }
    public CardInfoDto CardInfo { get; set; }

    public TransactionInfoDto() { }

    public TransactionInfoDto(string merchantId, string transactionId, bool success, PaymentInfoDto paymentInfo, CardInfoDto cardInfo)
    {
      MerchantId = merchantId;
      TransactionId = transactionId;
      Success = success;
      PaymentInfo = paymentInfo;
      CardInfo = cardInfo;
    }

    public TransactionInfoDto(string merchantId, string transactionId, bool success, double amount, string currency, long cardNumber, DateTime expiryDate, int cvv)
    {
      MerchantId = merchantId;
      TransactionId = transactionId;
      Success = success;
      PaymentInfo = new PaymentInfoDto(amount, currency);
      CardInfo = new CardInfoDto(cardNumber, expiryDate, cvv);
    }
    public override string ToString()
    {
      var result = JsonConvert.SerializeObject(this);
      return result;
    }
  }
}
