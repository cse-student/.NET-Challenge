using Couchbase.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.N1QL;
using Newtonsoft.Json;
using PaymentSystem.Core.Domain.EntityFramework.Dto;
using PaymentSystem.Core.Domain.EntityFramework.Repositories;

namespace PaymentSystem.Infrastructure.Domain.Repositories
{
  public class CouchbasePaymentRepository : IPaymentRepository
  {
    private IBucket Bucket { get; set; }

    public CouchbasePaymentRepository()
    {
      Bucket = ClusterHelper.GetBucket("transactions");
    }

    public async Task<TransactionInfoDto> StoreTransaction(TransactionInfoDto transactionInfo)
    {
      var statement = Constants.Queries.StoreTransaction;
      var key = Guid.NewGuid();
      var doc = new Document<TransactionInfoDto>{Id = key.ToString(), Content = transactionInfo};
      var result = await Bucket.InsertAsync<TransactionInfoDto>(doc);
      return result.Content;
    }

    public async Task<TransactionInfoDto> GetTransaction(string merchantId, string transactionId)
    {
      var statement = Constants.Queries.GetTransaction;
      statement = statement.Replace(Constants.PlaceHolders.TransactionId, transactionId);
      statement = statement.Replace(Constants.PlaceHolders.MerchantId, merchantId);
      var queryRequest = QueryRequest.Create(statement);
      queryRequest.ScanConsistency(ScanConsistency.RequestPlus);
      var result = await Bucket.QueryAsync<TransactionInfoDto>(queryRequest);
      return result.Rows?.FirstOrDefault();
    }

    public async Task<TransactionInfoDto[]> GetTransactions(string merchantId)
    {
      var statement = Constants.Queries.GetTransactions;
      statement = statement.Replace(Constants.PlaceHolders.MerchantId, merchantId);
      var queryRequest = QueryRequest.Create(statement);
      queryRequest.ScanConsistency(ScanConsistency.RequestPlus);
      var result = await Bucket.QueryAsync<TransactionInfoDto>(queryRequest);
      return result.Rows.ToArray();
    }
  }
}
