using System;
using System.Threading.Tasks;
using PaymentSystem.Core.Models;

namespace PaymentSystem.Core.Domain.Providers
{
  public interface IBankProvider
  {
    Task<BankResponse> ProcessTransaction(string accountNumber, long cardNumber, int cvv, double amount, DateTime expiryDate, string currency);
  }
}
