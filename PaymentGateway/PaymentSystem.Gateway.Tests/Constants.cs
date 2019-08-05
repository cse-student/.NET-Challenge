using System;
using System.Collections.Generic;
using System.Text;
using PaymentSystem.Core.Domain.EntityFramework.Dto;
using PaymentSystem.Core.Models;

namespace PaymentSystem.Gateway.Tests
{
  public class Constants
  {
    public class Transactions
    {
      public static TransactionInfoDto TransactionInfoDto1 = new TransactionInfoDto("f2d11dfd-7c4c-4640-a751-1481916493cf", "14160d13-0b15-400a-b5fc-bf203b4f7f80", true, 1000.00, "$", 4812341108713667, DateTime.Now.AddYears(1), 123);
      public static TransactionInfoDto TransactionInfoDto2 = new TransactionInfoDto("f2d11dfd-7c4c-4640-a751-1481916493cf", "15160d15-0b15-489a-b5cf-fb2034bff708", false, 1000000.00, "$", 8412314408717737, DateTime.Now.AddYears(1), 321);
    }

    public class Ids
    {
      public const string MerchantId = "f2d11dfd-7c4c-4640-a751-1481916493cf";
      public const string TransactionId = "14160d13-0b15-400a-b5fc-bf203b4f7f80";
    }

    public class Responses
    {
      public static BankResponse BankResponseSuccess = new BankResponse
      {
        TransactionId = Constants.Transactions.TransactionInfoDto1.TransactionId,
        Success = true
      };

      public static BankResponse BankResponseFailure = new BankResponse
      {
        TransactionId = Constants.Transactions.TransactionInfoDto1.TransactionId,
        Success = true
      };
    }

    public const string MerchantSecret = "86xLOo1uHFQOKft";
    public const string AccountNumber = "Fc0CLLq0oo";
  }
}
