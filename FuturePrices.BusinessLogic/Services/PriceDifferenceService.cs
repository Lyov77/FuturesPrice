using FuturesPrice.BusinessLogic.Interfaces;
using FuturesPrice.DAL.Interfaces;
using FuturesPrice.Shared.Models;

public class PriceDifferenceService : IPriceDifferenceService
{
    private readonly IPriceDifferenceValidator _validator;
    private readonly IPriceDifferenceCalculator _calculator;
    private readonly IPriceSaveRepository _priceSaveRepository;
    private readonly ILoggingService _loggingService;

    public PriceDifferenceService(
        IPriceDifferenceValidator validator,
        IPriceDifferenceCalculator calculator,
        IPriceSaveRepository priceSaveRepository,
        ILoggingService loggingService)
    {
        _validator = validator;
        _calculator = calculator;
        _priceSaveRepository = priceSaveRepository;
        _loggingService = loggingService;
    }

    public async Task<PriceDifferenceDto> CalculatePriceDifferenceAsync(string symbol, DateTime startDate, DateTime endDate)
    {
        try
        {
            await _loggingService.LogInfoAsync($"Начало расчета разницы цен для {symbol} с {startDate} по {endDate}");

            // symbol and date validation
            _validator.ValidateSymbol(symbol);
            _validator.ValidateDateRange(startDate, endDate);

            var startDateUtc = new DateTimeOffset(startDate).ToUniversalTime();
            var endDateUtc = new DateTimeOffset(endDate).ToUniversalTime();

            // price difference calculation
            var priceDifference = await _calculator.CalculateAsync(symbol, startDate, endDate);
                      
                        
            await _priceSaveRepository.SavePriceAsync(new FuturesPriceModel
            {
                Symbol = symbol,
                StartDate = startDateUtc,
                EndDate = endDateUtc,
                PriceDifference = priceDifference
            });
            
            // logging successful result
            await _loggingService.LogInfoAsync($"Ценовая разница для {symbol} с {startDate} по {endDate} равна {priceDifference}");

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
