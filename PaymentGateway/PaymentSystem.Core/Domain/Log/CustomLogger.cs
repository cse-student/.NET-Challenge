using System;
using Microsoft.Extensions.Logging;
using PaymentSystem.Core.Domain.EntityFramework.Dto;
using PaymentSystem.Core.Domain.EntityFramework.Repositories;

namespace PaymentSystem.Core.Domain.Log
{
  public class CustomLogger: ILogger
  {
    private readonly ILogRepository _logRepository;

    public CustomLogger(ILogRepository logRepository)
    {
      _logRepository = logRepository;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
      var log = new LogDto
      {
        TimeStamp = DateTime.Now.ToString("yyyy/mm/dd HH:mm"),
        LogMessage = state.ToString()
      };
      _logRepository.WriteLog(log);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
      throw new NotImplementedException();
    }

    public IDisposable BeginScope<TState>(TState state)
    {
      throw new NotImplementedException();
    }
  }
}
