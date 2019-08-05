using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSimulator.Models
{
  public class Response
  {
    public string TransactionId { get; set; }
    public bool Success { get; set; }
  }
}
