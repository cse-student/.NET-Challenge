namespace PaymentSystem.Core.Domain.EntityFramework.Dto
{
  public class PaymentInfoDto
  {
    public double Amount { get; set; }
    public string Currency { get; set; }

    public PaymentInfoDto() { }

    public PaymentInfoDto(double amount, string currency)
    {
      Amount = amount;
      Currency = currency;
    }
  }
}
