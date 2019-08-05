using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Core.Configuration;
using PaymentSystem.Core.Domain.EntityFramework.Dto;
using PaymentSystem.Core.Domain.EntityFramework.Repositories;
using PaymentSystem.Core.Domain.Providers;
using PaymentSystem.Core.Domain.StateManagement;
using PaymentSystem.Gateway.Models;

namespace PaymentSystem.Gateway.Controllers
{
  [Route("[controller]/[action]")]
  [ApiController]
  public class GatewayController : Controller
  {
    private readonly ICacheManager _cacheManager;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBankProvider _bankProvider;

    public GatewayController(ICacheManager cacheManager, IPaymentRepository paymentRepository, IBankProvider bankProvider)
    {
      _cacheManager = cacheManager;
      _paymentRepository = paymentRepository;
      _bankProvider = bankProvider;
    }

    // GET api/values
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
      return new string[] { "Welcome To Payment Gateway"};
    }

    // POST api/values
    [HttpPost]
    public async Task<IActionResult> ProcessPayment(long cardNumber, DateTime expiryDate, double amount, string currency, int cvv, string merchantId, string merchantSecret)
    {
      var result = new ProcessPaymentResponse {Success = false};
      if (string.IsNullOrEmpty(merchantId))
      {
        result.ErrorMessage = "parameter \"merchantId\" is mandatory";
        return StatusCode((int)HttpStatusCode.Unauthorized, result);
      }
      MerchantSettings merchantSettings;
      
      try
      {
        merchantSettings = _cacheManager.GetMerchantSettings(merchantId);
      }
      catch
      {
        var id = Guid.NewGuid();
        result.ErrorMessage = "An error occured. Please contact admin with id: " + id;
        return StatusCode((int)HttpStatusCode.InternalServerError, result);
      }
      if (merchantSettings == null)
      {
        result.ErrorMessage = "Unrecognized merchant";
        return StatusCode((int)HttpStatusCode.Unauthorized, result);
      }

      var response = await _bankProvider.ProcessTransaction(merchantSettings.AccountNumber, cardNumber, cvv, amount, expiryDate,  currency);
      result.Success = response.Success;
      var transactionInfo = new TransactionInfoDto(
        merchantId, response.TransactionId, response.Success, amount, currency, cardNumber,
        expiryDate, cvv);
      var storedTransaction = await _paymentRepository.StoreTransaction(transactionInfo);
      result.TransactionInfo = storedTransaction;
      return StatusCode((int)HttpStatusCode.OK, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetTransaction(string merchantId, string transactionId)
    {
      var result = await _paymentRepository.GetTransaction(merchantId, transactionId);
      return StatusCode((int)HttpStatusCode.OK, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(string merchantId)
    {
      var result = await _paymentRepository.GetTransactions(merchantId);
      return StatusCode((int)HttpStatusCode.OK, result);
    }
  }
}
