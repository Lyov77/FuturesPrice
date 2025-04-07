using FuturesPrice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesPrice.DAL.Interfaces
{
    public interface IPriceSaveRepository
    {
        Task SavePriceAsync(FuturesPriceModel price);
    }
}
