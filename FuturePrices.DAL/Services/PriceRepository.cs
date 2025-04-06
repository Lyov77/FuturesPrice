using FuturesPrice.DAL.Interfaces;
using FuturesPrice.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace FuturesPrice.DAL.Services
{
    public class PriceRepository : IPriceRepository
    {
        private readonly AppDbContext _context;

        public PriceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SavePriceAsync(FuturesPriceModel price)
        {
            _context.FuturesPrices.Add(price);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FuturesPriceModel>> GetPricesAsync(string symbol, DateTime from, DateTime to)
        {
            return await _context.FuturesPrices
                .Where(p => p.Symbol == symbol && p.Timestamp >= from && p.Timestamp <= to)
                .OrderBy(p => p.Timestamp)
                .ToListAsync();
        }
    }
}
