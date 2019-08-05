namespace PaymentSystem.Infrastructure
{
  public class Constants
  {

    public static class Queries
    {
      public static string GetTransactions = $"Select g.*, META(g).id From transactions g where merchantId = '{PlaceHolders.MerchantId}';";
      public static string GetTransaction = $"Select g.*, META(g).id From transactions g where transactionId = '{PlaceHolders.TransactionId}' and merchantId = '{PlaceHolders.MerchantId}';";
      public static string StoreTransaction = $"Insert Into transactions ( KEY, VALUE ) Values('{PlaceHolders.Key}',{ PlaceHolders.Document}) RETURNING  *;";
    }

    public static class PlaceHolders
    {
      public const string MerchantId = "$merchantId";
      public const string TransactionId = "$transactionId";
      public const string Key = "$key";
      public const string Document = "$document";
    }
  }
}
