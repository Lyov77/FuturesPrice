using FuturesPrice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesPrice.BusinessLogic.Interfaces
{
    public interface IPriceDifferenceService
    {
        Task<PriceDifferenceDto> CalculatePriceDifferenceAsync(string symbol, DateTime startDate, DateTime endDate);
    }
}
