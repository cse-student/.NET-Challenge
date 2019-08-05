using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using PaymentSystem.Core.Configuration;

namespace PaymentSystem.Core.Domain.StateManagement
{
  public class CacheManager: ICacheManager
  {
    private IMemoryCache _cache;

    private static readonly Lazy<CacheManager> lazy = new Lazy<CacheManager>(() => new CacheManager());
    public static CacheManager Instance => lazy.Value;

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
      _cache.Set(Core.Constants.CacheKey.Merchants, dictionary);
    }

    /// <summary>
    /// Retrieves merchant settings from dictionary stored in cache
    /// </summary>
    /// <param name="merchantId"></param>
    /// <returns></returns>
    public MerchantSettings GetMerchantSettings(string merchantId)
    {
      if (string.IsNullOrWhiteSpace(merchantId)) return null;
      var merchants = _cache.Get<Dictionary<string, MerchantSettings>>(Core.Constants.CacheKey.Merchants);
      if (merchants == null) throw new Exception();
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
        _cache.Set(Core.Constants.CacheKey.BankSettings, bankSettings);
      }
    }

    /// <summary>
    /// Retrieves bank api settings from cache
    /// </summary>
    /// <returns></returns>
    public BankSettings GetBankSettings()
    {
      return _cache.Get<BankSettings>(Core.Constants.CacheKey.BankSettings);
    }
  }
}
