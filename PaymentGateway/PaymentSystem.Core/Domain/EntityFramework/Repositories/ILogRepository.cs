using PaymentSystem.Core.Domain.EntityFramework.Dto;

namespace PaymentSystem.Core.Domain.EntityFramework.Repositories
{
  public interface ILogRepository
  {
    void WriteLog(LogDto logDto);
  }
}
