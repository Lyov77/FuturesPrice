using FuturesPrice.DAL.Interfaces;
using FuturesPrice.Shared.Models;

namespace FuturesPrice.DAL.Services
{
    public class LogRepository : ILogRepository
    {
        private readonly AppDbContext _context;

        public LogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveLogAsync(LogEntry log)
        {
            try
            {
                _context.LogEntries.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error saving log entry to the database", ex);
            }
        }

    }
}
