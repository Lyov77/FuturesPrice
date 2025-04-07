using FuturesPrice.Binance.Interfaces;
using FuturesPrice.BusinessLogic.Interfaces;
using FuturesPrice.DAL.Interfaces;

namespace FuturesPrice.BusinessLogic.Services
{
    public class PriceDifferenceCalculator : IPriceDifferenceCalculator
    {
        private readonly IBinanceService _binanceService;
        private readonly IPriceQueryRepository _priceQueryRepository;
        private readonly ILoggingService _loggingService;

        public PriceDifferenceCalculator(IBinanceService binanceService, IPriceQueryRepository priceQueryRepository, ILoggingService loggingService)
        {
            _binanceService = binanceService;
            _priceQueryRepository = priceQueryRepository;
            _loggingService = loggingService;
        }

        public async Task<decimal> CalculateAsync(string symbol, DateTime startDate, DateTime endDate)
        {

            decimal priceStart = await _binanceService.GetFuturePriceAsync(symbol, startDate);

            decimal priceEnd = await _binanceService.GetFuturePriceAsync(symbol, endDate);

            if (priceEnd == 0)
            {
                var lastPrice = await _priceQueryRepository.GetPricesAsync(symbol, endDate.AddDays(-1), endDate);
                priceEnd = lastPrice?.LastOrDefault()?.PriceDifference ?? priceStart;
                await _loggingService.LogErrorAsync($"Цена на {endDate} равна 0, использована последняя доступная цена: {priceEnd}");
            }

            var priceDifference = priceEnd - priceStart;

            return priceDifference;
        }
    }

}
