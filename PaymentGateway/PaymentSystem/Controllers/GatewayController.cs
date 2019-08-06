using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentSystem.Core.Domain.EntityFramework.Dto;
using PaymentSystem.Core.Domain.EntityFramework.Repositories;
using PaymentSystem.Core.Domain.Providers;
using PaymentSystem.Core.Domain.StateManagement;
using PaymentSystem.Core.Helpers;
using PaymentSystem.Gateway.Domain.ActionFilters;
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
    private readonly ILogger _logger;

    public GatewayController(ICacheManager cacheManager, IPaymentRepository paymentRepository,
      IBankProvider bankProvider, ILogger logger)
    {
      _cacheManager = cacheManager;
      _paymentRepository = paymentRepository;
      _bankProvider = bankProvider;
      _logger = logger;
    }

    /// <summary>
    /// Controller action for unauthorized access
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult AuthenticationFailed(string message)
    {
      var result = new ProcessPaymentResponse { ErrorMessage = message };
      return StatusCode((int)HttpStatusCode.Unauthorized, result);
    }

    /// <summary>
    /// Controller action responsible to process a transaction and storing the latter
    /// </summary>
    /// <param name="cardNumber"></param>
    /// <param name="expiryDate"></param>
    /// <param name="amount"></param>
    /// <param name="currency"></param>
    /// <param name="cvv"></param>
    /// <param name="merchantId"></param>
    /// <param name="merchantSecret"></param>
    /// <returns></returns>
    [AuthenticationActionFilter]
    [HttpPost]
    public async Task<IActionResult> ProcessPayment(long cardNumber, DateTime expiryDate, double amount,
      string currency, int cvv, string merchantId, string merchantSecret)
    {
      Guid id;
      try
      {
        var result = new ProcessPaymentResponse { Success = false };
        var merchantSettings = _cacheManager.GetMerchantSettings(merchantId);

        if (merchantSettings == null)
        {
          id = Guid.NewGuid();
          result.ErrorMessage = "An error occured. Please contact admin with id: " + id;
          var logMessage = $"[Exception:{id}]: Merchant Settings not being populated in cache";
          _logger.LogInformation(logMessage);
          return StatusCode((int)HttpStatusCode.InternalServerError, result);
        }
        var response = await _bankProvider.ProcessTransaction(merchantSettings.AccountNumber, cardNumber, cvv, amount,
          expiryDate, currency);
        result.Success = response.Success;
        var transactionInfo = new TransactionInfoDto(
          merchantId, response.TransactionId, response.Success, amount, currency, cardNumber,
          expiryDate, cvv);
        var storedTransaction = await _paymentRepository.StoreTransaction(transactionInfo);
        result.TransactionInfo = storedTransaction;
        return StatusCode((int)HttpStatusCode.OK, result);
      }
      catch (Exception ex)
      {
        id = Guid.NewGuid();
        _logger.LogInformation(string.Format(Core.Constants.Log.LogFormat, id, merchantId, ex.Message));
        _logger.LogInformation(string.Format(Core.Constants.Log.LogFormat, id, merchantId, ex.InnerException?.Message));
      }
      return StatusCode((int)HttpStatusCode.InternalServerError, $"An internal error occured. Please contact admin with id {id}");
    }

    /// <summary>
    /// Controller action which retrieves a transaction using the merchantId and transactionId
    /// </summary>
    /// <param name="merchantId"></param>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    [AuthenticationActionFilter]
    [HttpGet]
    public async Task<IActionResult> GetTransaction(string merchantId, string transactionId)
    {
      Guid id;
      try
      {
        var result = await _paymentRepository.GetTransaction(merchantId, transactionId);
        SecurityHelper.MaskCardNumber(result.CardInfo);
        return StatusCode((int)HttpStatusCode.OK, result);
      }
      catch (Exception ex)
      {
        id = Guid.NewGuid();
        _logger.LogInformation(string.Format(Core.Constants.Log.LogFormat, id, merchantId, ex.Message));
        _logger.LogInformation(string.Format(Core.Constants.Log.LogFormat, id, merchantId, ex.InnerException?.Message));
      }
      return StatusCode((int)HttpStatusCode.InternalServerError, $"An internal error occured. Please contact admin with id {id}");
    }

    /// <summary>
    /// Gets all transactions of a merchant using the param merchantId
    /// </summary>
    /// <param name="merchantId"></param>
    /// <returns></returns>
    [AuthenticationActionFilter]
    [HttpGet]
    public async Task<IActionResult> GetTransactions(string merchantId)
    {
      Guid id;
      try
      {
        var result = await _paymentRepository.GetTransactions(merchantId);
        foreach (var transactionInfo in result)
        {
          SecurityHelper.MaskCardNumber(transactionInfo.CardInfo);
        }
        return StatusCode((int)HttpStatusCode.OK, result);
      }
      catch (Exception ex)
      {
        id = Guid.NewGuid();
        _logger.LogInformation(string.Format(Core.Constants.Log.LogFormat, id, merchantId, ex.Message));
        _logger.LogInformation(string.Format(Core.Constants.Log.LogFormat, id, merchantId, ex.InnerException?.Message));
      }
      return StatusCode((int)HttpStatusCode.InternalServerError, $"An internal error occured. Please contact admin with id {id}");
    }
  }
}
