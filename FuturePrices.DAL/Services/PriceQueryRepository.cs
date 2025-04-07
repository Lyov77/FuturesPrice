using FuturesPrice.DAL.Interfaces;
using FuturesPrice.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace FuturesPrice.DAL.Services
{
    public class PriceQueryRepository : IPriceQueryRepository
    {
        private readonly AppDbContext _context;

        public PriceQueryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FuturesPriceModel>> GetPricesAsync(string symbol, DateTime start, DateTime end)
        {
            if (start == default || end == default)
                throw new ArgumentException("Invalid date range.", nameof(start));

            var fromUtc = new DateTimeOffset(start).ToUniversalTime();
            var toUtc = new DateTimeOffset(end).ToUniversalTime();

            return await _context.FuturesPrices
                .Where(p => p.Symbol == symbol && p.StartDate >= fromUtc && p.EndDate <= toUtc)
                .OrderBy(p => p.Id)
                .ToListAsync();
        }
    }
}
