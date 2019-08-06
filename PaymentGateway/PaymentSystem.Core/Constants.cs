namespace PaymentSystem.Core
{
  public static class Constants
  {
    public static class Configuration
    {
      public const string MerchantSettings = "MerchantsSettings";
      public const string BankSettings = "BankSettings";
      public const string AuthenticationSettings = "AuthenticationSettings";
      public const string Couchbase = "Couchbase";
    }

    public static class CacheKey
    {
      public const string Merchants = "Cache_Merchants";
      public const string BankSettings = "Cache_BankSettings";
      public const string AuthenticationSettings = "Cache_AuthenticationFailure";
    }

    public class QueryParams
    {
      public const string MerchantId = "merchantId";
      public const string MerchantSecret = "merchantSecret";
    }

    public class RoutingKeys
    {
      public const string Controller = "Controller";
      public const string Action = "Action";
      public const string Message = "message";
    }

    public class ErrorMessages
    {
      public const string MandatoryMerchantId = "param merchant id is mandatory";
      public const string MandatoryMerchantSecret = "param merchant secret is mandatory";
      public const string AuthenticationFailed = "Authentication Failed";
    }

    public class Log
    {
      public const string LogFormat = "[Exception:{0}][MerchantId:{1}] Message:{2}";
    }
  }
}
