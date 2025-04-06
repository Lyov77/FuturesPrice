using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesPrice.Shared.Models
{
    public class PriceDifferenceDto
    {
        public DateTime Timestamp { get; set; }
        public decimal PriceDifference { get; set; }
    }
}
