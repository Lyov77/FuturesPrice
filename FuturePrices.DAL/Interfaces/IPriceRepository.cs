using FuturesPrice.Shared.Models;

namespace FuturesPrice.DAL.Interfaces
{
    public interface IPriceRepository
    {
        Task SavePriceAsync(FuturesPriceModel price);
        Task<IEnumerable<FuturesPriceModel>> GetPricesAsync(string symbol, DateTime from, DateTime to);
    }
}
