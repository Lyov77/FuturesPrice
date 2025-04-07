using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesPrice.BusinessLogic.Interfaces
{
    public interface IPriceDifferenceCalculator
    {
        Task<decimal> CalculateAsync(string symbol, DateTime startDate, DateTime endDate);
    }
}
