using System;

namespace PaymentSystem.Core.Domain.EntityFramework.Dto
{
  public class CardInfoDto
  {
    public long CardNumber { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int Cvv { get; set; }
    public string MaskedCardNumber { get; set; }

    public CardInfoDto(){}

    public CardInfoDto(long cardNumber, DateTime expiryDate, int cvv)
    {
      CardNumber = cardNumber;
      ExpiryDate = expiryDate;
      Cvv = cvv;
    }
  }
}
