using FuturesPrice.DAL.Interfaces;
using FuturesPrice.Shared.Models;


namespace FuturesPrice.DAL.Services
{
    public class PriceSaveRepository : IPriceSaveRepository
    {
        private readonly AppDbContext _context;

        public PriceSaveRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SavePriceAsync(FuturesPriceModel price)
        {
            if (price == null)
                throw new ArgumentNullException(nameof(price));

            price.StartDate = price.StartDate.ToUniversalTime();
            price.EndDate = price.EndDate.ToUniversalTime();

            _context.FuturesPrices.Add(price);
            await _context.SaveChangesAsync();
        }
    }

}

