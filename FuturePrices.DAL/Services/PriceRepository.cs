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
            // данные в UTC перед сохранением
            price.StartDate = price.StartDate.ToUniversalTime();
            price.EndDate = price.EndDate.ToUniversalTime();

            _context.FuturesPrices.Add(price);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FuturesPriceModel>> GetPricesAsync(string symbol, DateTime from, DateTime to)
        {
            
            var fromUtc = new DateTimeOffset(from).ToUniversalTime();
            var toUtc = new DateTimeOffset(to).ToUniversalTime();

            return await _context.FuturesPrices
                .Where(p => p.Symbol == symbol && p.StartDate >= fromUtc && p.EndDate <= toUtc)
                .OrderBy(p => p.Id)
                .ToListAsync();
        }
    }
}
