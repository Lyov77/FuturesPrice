using FuturesPrice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesPrice.DAL.Interfaces
{
    public interface IPriceQueryRepository
    {
        Task<IEnumerable<FuturesPriceModel>> GetPricesAsync(string symbol, DateTime start, DateTime end);
    }
}
