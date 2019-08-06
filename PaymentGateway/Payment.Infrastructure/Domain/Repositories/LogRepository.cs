using System;
using Couchbase;
using Couchbase.Core;
using PaymentSystem.Core.Domain.EntityFramework.Dto;
using PaymentSystem.Core.Domain.EntityFramework.Repositories;

namespace PaymentSystem.Infrastructure.Domain.Repositories
{
  public class LogRepository: ILogRepository
  {
    private IBucket Bucket { get; }

    public LogRepository()
    {
      Bucket = ClusterHelper.GetBucket(Constants.BucketsName.Logs);
    }

    public async void WriteLog(LogDto logDto)
    {
      var statement = Constants.Queries.StoreTransaction;
      var key = Guid.NewGuid();
      var doc = new Document<LogDto> { Id = key.ToString(), Content = logDto };
      var result = await Bucket.InsertAsync<LogDto>(doc);
    }
  }
}
