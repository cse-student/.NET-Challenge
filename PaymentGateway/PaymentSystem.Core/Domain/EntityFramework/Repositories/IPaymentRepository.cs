using System.Threading.Tasks;
using PaymentSystem.Core.Domain.EntityFramework.Dto;

namespace PaymentSystem.Core.Domain.EntityFramework.Repositories
{
  public interface IPaymentRepository
  {
    Task<TransactionInfoDto> StoreTransaction(TransactionInfoDto transactionInfo);
    Task<TransactionInfoDto> GetTransaction(string merchantId, string transactionId);
    Task<TransactionInfoDto[]> GetTransactions(string merchantId);
  }
}
