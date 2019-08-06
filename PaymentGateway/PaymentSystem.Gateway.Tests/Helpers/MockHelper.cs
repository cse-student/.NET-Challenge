using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentSystem.Core.Configuration;
using PaymentSystem.Core.Domain.EntityFramework.Dto;
using PaymentSystem.Core.Domain.EntityFramework.Repositories;
using PaymentSystem.Core.Domain.Providers;
using PaymentSystem.Core.Domain.StateManagement;
using PaymentSystem.Gateway.Controllers;

namespace PaymentSystem.Gateway.Tests.Helpers
{
  public static class MockHelper
  {
    public static GatewayController GetGatewayControllerMock()
    {
      var result = new GatewayController
        (GetCacheManagerMock(), GetPaymentRepositoryMock(), GetBankProviderMock(), GetCustomLoggerMock());
      return result;
    }

    public static ICacheManager GetCacheManagerMock()
    {
      var cacheManagerMock = new Mock<ICacheManager>();
      cacheManagerMock.Setup(p => p.GetMerchantSettings(Constants.Ids.MerchantId)).
        Returns(new MerchantSettings
        {
          MerchantId = Constants.Ids.MerchantId,
          MerchantSecret = "86xLOo1uHFQOKft",
          MerchantKey = "eTzr7Jz0dOcn7R1",
          MerchantHashedSecret = "5zjGlvqg0gxMVUR4xyjNl34D7tD+OKKzJCPTATIeNrw=",
          AccountNumber = "SD8qnKEDBoXb8dq"
        });
      return cacheManagerMock.Object;
    }

    public static IPaymentRepository GetPaymentRepositoryMock()
    {
      var paymentRepositoryMock = new Mock<IPaymentRepository>();

      paymentRepositoryMock.Setup(p => p.GetTransaction(Constants.Ids.MerchantId, Constants.Ids.TransactionId)).
        Returns(Task.FromResult(Constants.Transactions.TransactionInfoDto1));
      paymentRepositoryMock.Setup(p => p.GetTransactions(Constants.Ids.MerchantId)).
        Returns(Task.FromResult(Constants.Transactions.TransactionsArray));
      paymentRepositoryMock.Setup(x => x.StoreTransaction(It.IsAny<TransactionInfoDto>())).Returns(Task.FromResult(Constants.Transactions.TransactionInfoDto1));
      return paymentRepositoryMock.Object;
    }

    public static IBankProvider GetBankProviderMock()
    {
      var bankProviderMock = new Mock<IBankProvider>();
      bankProviderMock.Setup(p => p.ProcessTransaction(Constants.AccountNumber, Constants.Transactions.TransactionInfoDto1.CardInfo.CardNumber, Constants.Transactions.TransactionInfoDto1.CardInfo.Cvv, Constants.Transactions.TransactionInfoDto1.PaymentInfo.Amount, Constants.Transactions.TransactionInfoDto1.CardInfo.ExpiryDate, Constants.Transactions.TransactionInfoDto1.PaymentInfo.Currency)).
        Returns(Task.FromResult(Constants.Responses.BankResponseSuccess));
      return bankProviderMock.Object;
    }

    public static ILogger GetCustomLoggerMock()
    {
      var customLoggerMock = new Mock<ILogger>();
      return customLoggerMock.Object;
    }
  }
}
