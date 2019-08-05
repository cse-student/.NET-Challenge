using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentSystem.Core.Domain.Providers;
using PaymentSystem.Core.Domain.StateManagement;
using PaymentSystem.Core.Models;

namespace PaymentSystem.BankProvider
{
  public class BankProvider: IBankProvider
  {
    private static HttpClient Client { get; set; }
    private readonly ICacheManager _cacheManager;

    public BankProvider(ICacheManager cacheManager)
    {
      _cacheManager = cacheManager;
      Client = new HttpClient();
    }

    public async Task<BankResponse> ProcessTransaction(string accountNumber, long cardNumber, int cvv, double amount, DateTime expiryDate, string currency)
    {
      var values = new Dictionary<string, string>
      {
        { Constants.Parameters.AccountNumber, accountNumber },
        { Constants.Parameters.CardNumber, cardNumber.ToString() },
        { Constants.Parameters.Cvv, cvv.ToString() },
        { Constants.Parameters.Amount, amount.ToString(CultureInfo.InvariantCulture) },
        { Constants.Parameters.ExpiryDate, expiryDate.ToString("MM/YYYY") },
        { Constants.Parameters.Currency, currency }
      };

      var content = new FormUrlEncodedContent(values);
      var bankSettings = _cacheManager.GetBankSettings();
      if (bankSettings == null || string.IsNullOrWhiteSpace(bankSettings.ApiUrl))
      {
        throw new Exception();
      }
      BankResponse bankResponse = null ;
      var responseString = string.Empty;
      try
      {
        using (var httpClientHandler = new HttpClientHandler())
        {
          httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
          using (var client = new HttpClient(httpClientHandler))
          {
            var response = await client.PostAsync(bankSettings.ApiUrl, content);
            responseString = await response.Content.ReadAsStringAsync();
          }
        }
        bankResponse = JsonConvert.DeserializeObject<BankResponse>(responseString);
      }
      catch (Exception ex)
      {

      }

      return bankResponse;
    }
  }
}
