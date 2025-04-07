using FuturesPrice.BusinessLogic.Interfaces;
using FuturesPrice.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace FuturesPrice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceDifferenceController : ControllerBase
    {
        private readonly IPriceDifferenceService _priceDifferenceService;

        public PriceDifferenceController(IPriceDifferenceService priceDifferenceService)
        {
            _priceDifferenceService = priceDifferenceService;
        }

        [HttpGet("calculate")]
        public async Task<IActionResult> CalculatePriceDifference(string symbol, DateTime startDate, DateTime endDate)
        {
            
            var priceDifference = await _priceDifferenceService.CalculatePriceDifferenceAsync(symbol, startDate, endDate);
                
            if (priceDifference != null)
                return Ok(priceDifference);
                
            return StatusCode(500, $"Internal server error");
            
        }
    }
}
