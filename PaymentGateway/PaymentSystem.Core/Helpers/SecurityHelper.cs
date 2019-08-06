using System;
using System.Security.Cryptography;
using System.Text;
using PaymentSystem.Core.Domain.EntityFramework.Dto;

namespace PaymentSystem.Core.Helpers
{
  public static class SecurityHelper
  {
    public static string Hash(string param, string key)
    {
      string result;
      var encoding = new ASCIIEncoding();
      var keyBytes = encoding.GetBytes(key);
      var paramBytes = encoding.GetBytes(param);
      using (var hashGenerator = new HMACSHA256(keyBytes))
      {
        var hashedParam = hashGenerator.ComputeHash(paramBytes);
        result = Convert.ToBase64String(hashedParam);
      }
      return result;
    }

    public static void MaskCardNumber(CardInfoDto cardInfo)
    {
      cardInfo.MaskedCardNumber = cardInfo.CardNumber.ToString();
      cardInfo.CardNumber = 0;
      var len = cardInfo.MaskedCardNumber.Length - 4;
      var result = new string('*', len);
      result = result + cardInfo.MaskedCardNumber.Substring(len);
      cardInfo.MaskedCardNumber = result;
      long.TryParse(cardInfo.MaskedCardNumber.Substring(len), out var number);
      cardInfo.CardNumber = number;
    }
  }
}
