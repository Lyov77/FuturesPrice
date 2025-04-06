using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesPrice.Binance.Interfaces
{
    public interface IBinanceService
    {
        Task<decimal> GetFuturePriceAsync(string symbol, DateTime timestamp);
    }
}
