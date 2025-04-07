using FuturesPrice.BusinessLogic.Interfaces;

namespace FuturesPrice.BusinessLogic.Services
{
    public class PriceDifferenceValidator : IPriceDifferenceValidator
    {
        private readonly ILoggingService _loggingService;

        public PriceDifferenceValidator(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void ValidateSymbol(string symbol)
        {

            if (string.IsNullOrEmpty(symbol) || !symbol.All(char.IsLetterOrDigit) || symbol.Length < 2)
            {
                var errorMessage = $"Некорректный символ: {symbol}";
                _loggingService.LogErrorAsync(errorMessage);
                throw new ArgumentException(errorMessage);
            }

        }

        public void ValidateDateRange(DateTime startDate, DateTime endDate)
        {

            if (startDate > endDate)
            {
                var errorMessage = $"Начальная дата ({startDate}) не может быть позже конечной даты ({endDate})";
                _loggingService.LogErrorAsync(errorMessage);
                throw new ArgumentException(errorMessage);
            }

        }
    }


}
