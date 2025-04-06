using FuturesPrice.BusinessLogic.Interfaces;
using FuturesPrice.Binance.Interfaces;
using FuturesPrice.DAL.Interfaces;
using FuturesPrice.Shared.Models;


namespace FuturesPrice.BusinessLogic
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
            // Получаем цены за предыдущие и текущие даты
            var prices = await _priceRepository.GetPricesAsync(symbol, startDate, endDate);

            // Если цены за начальный промежуток отсутствуют, получаем их с Binance
            decimal priceStart = prices.FirstOrDefault(p => p.Timestamp == startDate)?.Price
                ?? await _binanceService.GetFuturePriceAsync(symbol, startDate);

            decimal priceEnd = prices.FirstOrDefault(p => p.Timestamp == endDate)?.Price
                ?? await _binanceService.GetFuturePriceAsync(symbol, endDate);

            // Если цены за конец промежутка отсутствуют, используем цену за предыдущий день
            if (priceEnd == 0)
            {
                var lastPrice = await _priceRepository.GetPricesAsync(symbol, endDate.AddDays(-1), endDate);
                priceEnd = lastPrice?.LastOrDefault()?.Price ?? priceStart; // Используем цену за предыдущий день
            }

            // Сохраняем цены в базу данных
            await _priceRepository.SavePriceAsync(new FuturesPriceModel
            {
                Symbol = symbol,
                Timestamp = startDate,
                Price = priceStart
            });
            await _priceRepository.SavePriceAsync(new FuturesPriceModel
            {
                Symbol = symbol,
                Timestamp = endDate,
                Price = priceEnd
            });

            // Вычисляем разницу
            var priceDifference = priceEnd - priceStart;

            return new PriceDifferenceDto
            {
                Timestamp = endDate,
                PriceDifference = priceDifference
            };
        }
    }
}
