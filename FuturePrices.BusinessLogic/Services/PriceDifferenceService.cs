using Serilog;
using FuturesPrice.BusinessLogic.Interfaces;
using FuturesPrice.Shared.Models;
using FuturesPrice.DAL.Interfaces;
using FuturesPrice.Binance.Interfaces;

namespace FuturesPrice.BusinessLogic.Services
{
    public class PriceDifferenceService : IPriceDifferenceService
    {
        private readonly IBinanceService _binanceService;
        private readonly IPriceRepository _priceRepository;
        private readonly ILoggingService _loggingService;

        public PriceDifferenceService(IBinanceService binanceService, IPriceRepository priceRepository, ILoggingService loggingService)
        {
            _binanceService = binanceService;
            _priceRepository = priceRepository;
            _loggingService = loggingService;
        }

        public async Task<PriceDifferenceDto> CalculatePriceDifferenceAsync(string symbol, DateTime startDate, DateTime endDate)
        {
            try
            {
                await _loggingService.LogInfoAsync($"Начало расчета разницы цен для {symbol} с {startDate} по {endDate}");

                // Validate symbol format (example: should be uppercase, alphanumeric)
                if (string.IsNullOrEmpty(symbol) || !symbol.All(char.IsLetterOrDigit) || symbol.Length < 2)
                {
                    var errorMessage = $"Некорректный символ: {symbol}";
                    await _loggingService.LogErrorAsync(errorMessage);
                    throw new ArgumentException(errorMessage);
                }

                // Validate date format
                if (startDate > endDate)
                {
                    var errorMessage = $"Начальная дата ({startDate}) не может быть позже конечной даты ({endDate})";
                    await _loggingService.LogErrorAsync(errorMessage);
                    throw new ArgumentException(errorMessage);
                }

                var startDateUtc = new DateTimeOffset(startDate).ToUniversalTime();
                var endDateUtc = new DateTimeOffset(endDate).ToUniversalTime();

                var prices = await _priceRepository.GetPricesAsync(symbol, startDateUtc.DateTime, endDateUtc.DateTime);

                decimal priceStart = await _binanceService.GetFuturePriceAsync(symbol, startDate);

                decimal priceEnd = await _binanceService.GetFuturePriceAsync(symbol, endDate);

                if (priceEnd == 0)
                {
                    var lastPrice = await _priceRepository.GetPricesAsync(symbol, endDateUtc.AddDays(-1).DateTime, endDateUtc.DateTime);
                    priceEnd = lastPrice?.LastOrDefault()?.PriceDifference ?? priceStart;
                }

                var priceDifference = priceEnd - priceStart;

                // Log the success of price difference calculation
                await _loggingService.LogInfoAsync($"Ценовая разница для {symbol} с {startDate} по {endDate} равна {priceDifference}");

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
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync($"Ошибка при расчете разницы цен для {symbol} с {startDate} по {endDate}", ex);
                throw;
            }
        }
    }
}
