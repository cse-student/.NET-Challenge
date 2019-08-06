using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Core.Domain.EntityFramework.Dto;
using PaymentSystem.Gateway.Controllers;
using PaymentSystem.Gateway.Models;
using PaymentSystem.Gateway.Tests.Helpers;
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
    public async Task PostData_ReturnsStatus200()
    {
      var response = await _gatewayController.ProcessPayment(Constants.Transactions.TransactionInfoDto1.CardInfo.CardNumber,
        Constants.Transactions.TransactionInfoDto1.CardInfo.ExpiryDate,
        Constants.Transactions.TransactionInfoDto1.PaymentInfo.Amount,
        Constants.Transactions.TransactionInfoDto1.PaymentInfo.Currency,
        Constants.Transactions.TransactionInfoDto1.CardInfo.Cvv,
        Constants.Ids.MerchantId,
        Constants.MerchantSecret) as ObjectResult;
      Assert.Equal(200, response.StatusCode);
    }

    [Fact]
    public async Task PostData_ReturnsSuccess()
    { 
      var response = await _gatewayController.ProcessPayment(Constants.Transactions.TransactionInfoDto1.CardInfo.CardNumber, 
        Constants.Transactions.TransactionInfoDto1.CardInfo.ExpiryDate, 
        Constants.Transactions.TransactionInfoDto1.PaymentInfo.Amount, 
        Constants.Transactions.TransactionInfoDto1.PaymentInfo.Currency, 
        Constants.Transactions.TransactionInfoDto1.CardInfo.Cvv,
        Constants.Ids.MerchantId, 
        Constants.MerchantSecret) as ObjectResult;
      var value = response.Value as ProcessPaymentResponse;
      Assert.True(value.Success);
    }

    [Fact]
    public async Task PostData_ReturnsTransaction1()
    {
      var response = await _gatewayController.ProcessPayment(Constants.Transactions.TransactionInfoDto1.CardInfo.CardNumber,
        Constants.Transactions.TransactionInfoDto1.CardInfo.ExpiryDate,
        Constants.Transactions.TransactionInfoDto1.PaymentInfo.Amount,
        Constants.Transactions.TransactionInfoDto1.PaymentInfo.Currency,
        Constants.Transactions.TransactionInfoDto1.CardInfo.Cvv,
        Constants.Ids.MerchantId,
        Constants.MerchantSecret) as ObjectResult;
      var value = response.Value as ProcessPaymentResponse;
      Assert.Equal(value.TransactionInfo, Constants.Transactions.TransactionInfoDto1);
    }

    [Fact]
    public async Task GetTransaction_ReturnsStatus200()
    {
      var response = await _gatewayController.GetTransaction(Constants.Ids.MerchantId, Constants.Ids.TransactionId) as ObjectResult;
      Assert.Equal(200, response.StatusCode);
    }

    [Fact]
    public async Task GetTransaction_ReturnsTransaction1()
    {
      var response = await _gatewayController.GetTransaction(Constants.Ids.MerchantId, Constants.Ids.TransactionId) as ObjectResult;
      var transactionInfo = response.Value as TransactionInfoDto;
      Assert.Equal(transactionInfo, Constants.Transactions.TransactionInfoDto1);
    }

    [Fact]
    public async Task GetTransactions_ReturnsStatus200()
    {
      var response = await _gatewayController.GetTransactions(Constants.Ids.MerchantId) as ObjectResult;
      Assert.Equal(200, response.StatusCode);
    }

    [Fact]
    public async Task GetTransactions_ReturnsTransactions1And2()
    {
      var response = await _gatewayController.GetTransactions(Constants.Ids.MerchantId) as ObjectResult;
      var transactions = response.Value as TransactionInfoDto[];
      Assert.Equal(Constants.Transactions.TransactionsArray, transactions);
    }
  }
}
