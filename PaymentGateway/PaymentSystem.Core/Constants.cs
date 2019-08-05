namespace PaymentSystem.Core
{
  public static class Constants
  {
    public static class Configuration
    {
      public const string MerchantSettings = "MerchantsSettings";
      public const string BankSettings = "BankSettings";
      public const string Couchbase = "Couchbase";
    }

    public static class CacheKey
    {
      public const string Merchants = "Cache_Merchants";
      public const string BankSettings = "Cache_BankSettings";
    }
  }
}
