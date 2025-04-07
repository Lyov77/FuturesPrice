using FuturesPrice.DAL.Interfaces;
using FuturesPrice.Shared.Models;
using Microsoft.EntityFrameworkCore;

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
            _context.LogEntries.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
