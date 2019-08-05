using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Gateway.Controllers;
using Xunit;

namespace PaymentSystem.Gateway.Tests
{
  public class GatewayControllerUnitTest
  {
    private readonly GatewayController _gatewayController;

    public GatewayControllerUnitTest()
    {
      _gatewayController = MockHelper.GetGatewayControllerMock();
    }

    [Fact]
    public async Task PostData_401Forbidden_NoMerchantId()
    {
      var dateStr = "08/19";
      DateTime.TryParse(dateStr, out var date);
      //string cardNumber, DateTime expiryDate, double amount, string currency, int cvv, string merchantId, string merchantSecret
      var response = await _gatewayController.ProcessPayment(Constants.Transactions.TransactionInfoDto1.CardInfo.CardNumber, Constants.Transactions.TransactionInfoDto1.CardInfo.ExpiryDate, Constants.Transactions.TransactionInfoDto1.PaymentInfo.Amount, Constants.Transactions.TransactionInfoDto1.PaymentInfo.Currency, Constants.Transactions.TransactionInfoDto1.CardInfo.Cvv, "", Constants.MerchantSecret) as ObjectResult;
      Assert.Equal(401, response.StatusCode);
    }
  }
}
