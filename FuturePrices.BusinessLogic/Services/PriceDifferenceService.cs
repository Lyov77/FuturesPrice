using FuturesPrice.Binance.Interfaces;
using FuturesPrice.BusinessLogic.Interfaces;
using FuturesPrice.DAL.Interfaces;
using FuturesPrice.Shared.Models;

namespace FuturesPrice.BusinessLogic.Services
{
    public class PriceDifferenceService : IPriceDifferenceService
    {
        private readonly IBinanceService _binanceService;
        private readonly IPriceRepository _priceRepository;

        public PriceDifferenceService(IBinanceService binanceService, IPriceRepository priceRepository)
        {
            _binanceService = binanceService;
            _priceRepository = priceRepository;
        }

        public async Task<PriceDifferenceDto> CalculatePriceDifferenceAsync(string symbol, DateTime startDate, DateTime endDate)
        {
            // startDate и endDate в UTC
            var startDateUtc = new DateTimeOffset(startDate).ToUniversalTime();
            var endDateUtc = new DateTimeOffset(endDate).ToUniversalTime();

            // Получаем цены за предыдущие и текущие даты
            var prices = await _priceRepository.GetPricesAsync(symbol, startDateUtc.DateTime, endDateUtc.DateTime);

            // Если цены за начальный промежуток отсутствуют, получаем их с Binance
            decimal priceStart = prices.FirstOrDefault(p => p.StartDate == startDateUtc)?.PriceDifference
                ?? await _binanceService.GetFuturePriceAsync(symbol, startDate);

            decimal priceEnd = prices.FirstOrDefault(p => p.EndDate == endDateUtc)?.PriceDifference
                ?? await _binanceService.GetFuturePriceAsync(symbol, endDate);

            // Если цены за конец промежутка отсутствуют, используем цену за предыдущий день
            if (priceEnd == 0)
            {
                var lastPrice = await _priceRepository.GetPricesAsync(symbol, endDateUtc.AddDays(-1).DateTime, endDateUtc.DateTime);
                priceEnd = lastPrice?.LastOrDefault()?.PriceDifference ?? priceStart; // Используем цену за предыдущий день
            }

            var priceDifference = priceEnd - priceStart;

            // Сохраняем цены в базу данных
            await _priceRepository.SavePriceAsync(new FuturesPriceModel
            {
                Symbol = symbol,
                StartDate = startDateUtc,
                EndDate = endDateUtc,
                PriceDifference = priceDifference
            });

            return new PriceDifferenceDto
            {
                StartDate = startDateUtc,
                EndDate = endDateUtc,
                PriceDifference = priceDifference
            };
        }
    }
}
