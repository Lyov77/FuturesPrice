using FuturesPrice.DAL.Interfaces;
using FuturesPrice.Shared.Models;
using Serilog;
using FuturesPrice.BusinessLogic.Interfaces;

namespace FuturesPrice.BusinessLogic.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogRepository _logRepository;
        private readonly Serilog.ILogger _logger;

        public LoggingService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
            _logger = Log.ForContext<LoggingService>();
        }

        public async Task LogInfoAsync(string message)
        {
            _logger.Information(message);
            await SaveLogAsync("Information", message);
        }

        public async Task LogErrorAsync(string message, Exception ex = null)
        {
            _logger.Error(ex, message);
            await SaveLogAsync("Error", message, ex?.ToString());
        }

        public async Task SaveLogAsync(string level, string message, string exception = null)
        {
            

            var logEntry = new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = level,
                Message = message,
                Exception = exception,
                Properties = null
            };

            await _logRepository.SaveLogAsync(logEntry);
        }
    }
}
