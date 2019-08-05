using PaymentSystem.Core.Domain.EntityFramework.Dto;

namespace PaymentSystem.Gateway.Models
{
  public class ProcessPaymentResponse
  {
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public TransactionInfoDto TransactionInfo { get; set; }
  }
}
