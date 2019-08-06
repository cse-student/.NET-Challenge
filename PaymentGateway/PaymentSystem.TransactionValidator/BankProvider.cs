using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentSystem.Core.Domain.Providers;
using PaymentSystem.Core.Domain.StateManagement;
using PaymentSystem.Core.Models;

namespace PaymentSystem.BankProvider
{
  public class BankProvider: IBankProvider
  {
    private readonly ICacheManager _cacheManager;
    private readonly ILogger _logger;

    public BankProvider(ICacheManager cacheManager, ILogger logger)
    {
      _cacheManager = cacheManager;
      new HttpClient();
      _logger = logger;
    }

    /// <summary>
    /// Send transaction to bank to be process
    /// Url is retrieved from cache (was retrieved from applicationSettings.json file and populated in cache in ConfigureServices method)
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="cardNumber"></param>
    /// <param name="cvv"></param>
    /// <param name="amount"></param>
    /// <param name="expiryDate"></param>
    /// <param name="currency"></param>
    /// <returns></returns>
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
      try
      {
        string responseString;
        using (var httpClientHandler = new HttpClientHandler())
        {
          //Added to bypass SSL issue
          httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
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
        _logger.LogInformation($"[BankProvider] {ex.Message}");
        _logger.LogInformation($"[BankProvider] {ex.InnerException?.Message}");
      }

      return bankResponse;
    }
  }
}
