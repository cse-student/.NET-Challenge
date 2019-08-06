using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using PaymentSystem.Core.Configuration;

namespace PaymentSystem.Core.Domain.StateManagement
{
  public class CacheManager: ICacheManager
  {
    private IMemoryCache _cache;

    public Dictionary<string, MerchantSettings> MerchantDetails { get; set; }

    public IMemoryCache Cache {
      set => _cache = value;
    }

    /// <summary>
    /// Use merchant settings read json to populate a dictionary in cache memory using merchant id as key
    /// </summary>
    /// <param name="merchants">Merchant Settings read from json file</param>
    public void SetMerchantsSettings(MerchantSettings[] merchants)
    {
      var dictionary = new Dictionary<string, MerchantSettings>();
      if (merchants == null || merchants.Length < 1) return;
      foreach (var merchant in merchants)
      {
        dictionary.Add(merchant.MerchantId, merchant);
      }
      _cache.Set(Constants.CacheKey.Merchants, dictionary);
    }

    /// <summary>
    /// Retrieves merchant settings from dictionary stored in cache
    /// </summary>
    /// <param name="merchantId"></param>
    /// <returns></returns>
    public MerchantSettings GetMerchantSettings(string merchantId)
    {
      if (string.IsNullOrWhiteSpace(merchantId)) return null;
      var merchants = _cache.Get<Dictionary<string, MerchantSettings>>(Constants.CacheKey.Merchants);
      if (merchants == null) return null;
      merchants.TryGetValue(merchantId, out var result);
      return result;
    }

    /// <summary>
    /// Stores bank api settings in cache
    /// </summary>
    /// <param name="bankSettings"></param>
    public void SetBankSettings(BankSettings bankSettings)
    {
      if (bankSettings != null)
      {
        _cache.Set(Constants.CacheKey.BankSettings, bankSettings);
      }
    }

    /// <summary>
    /// Retrieves bank api settings from cache
    /// </summary>
    /// <returns></returns>
    public BankSettings GetBankSettings()
    {
      return _cache.Get<BankSettings>(Constants.CacheKey.BankSettings);
    }

    public void SetAuthenticationSettings(AuthenticationSettings authenticationSettings)
    {
      if (authenticationSettings != null)
      {
        _cache.Set(Constants.CacheKey.AuthenticationSettings, authenticationSettings);
      }
    }

    public AuthenticationSettings GetAuthenticationSettings()
    {
      return _cache.Get<AuthenticationSettings>(Constants.CacheKey.AuthenticationSettings);
    }
  }
}
