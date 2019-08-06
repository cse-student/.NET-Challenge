using System;
using System.Security.Cryptography;
using System.Text;

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
  }
}
