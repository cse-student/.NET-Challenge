using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Infrastructure
{
  public class Constants
  {

    public static class Queries
    {
      public static string GetTransactions = $"Select g.*, META(g).id From transactions g where MerchantId = {PlaceHolders.MerchantId};";
    }

    public static class PlaceHolders
    {
      public const string MerchantId = "{merchantId}";
    }
  }
}
