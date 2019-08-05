using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BankSimulator.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulator.Controllers
{
  [Route("[controller]/[action]")]
  [ApiController]
  public class SimulatorController : ControllerBase
  {
    private static bool Success { get; set; }

    [HttpGet]
    public IActionResult Get()
    {
      return StatusCode((int)HttpStatusCode.OK, "Welcome to Banking Simulator!");
    }
    // POST api/values
    [HttpPost]
    public IActionResult ProcessTransfer(string accountNumber, string cardNumber, int cvv, double amount, DateTime expiryDate, string currency)
    {
      Success = !Success;
      var transactionId = Guid.NewGuid();
      var response = new Response
      {
        TransactionId = transactionId.ToString(),
        Success = Success
      };
      return StatusCode((int)HttpStatusCode.OK, response);
    }
  }
}
