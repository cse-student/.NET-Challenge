using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using PaymentSystem.Core.Configuration;

namespace PaymentSystem.Core.Domain.StateManagement
{
  public interface ICacheManager
  {

    Dictionary<string, MerchantSettings> MerchantDetails { get; set; }
    IMemoryCache Cache { set; }
    void SetMerchantsSettings(MerchantSettings[] merchantSettings);
    MerchantSettings GetMerchantSettings(string merchantId);
    void SetBankSettings(BankSettings bankSettings);
    BankSettings GetBankSettings();
    void SetAuthenticationSettings(AuthenticationSettings authenticationSettings);
    AuthenticationSettings GetAuthenticationSettings();
  }
}
