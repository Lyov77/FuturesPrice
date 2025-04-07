namespace FuturesPrice.BusinessLogic.Interfaces
{
    public interface IPriceDifferenceValidator
    {
        void ValidateSymbol(string symbol);
        void ValidateDateRange(DateTime startDate, DateTime endDate);
    }

}
