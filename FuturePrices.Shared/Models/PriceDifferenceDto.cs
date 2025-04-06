namespace FuturesPrice.Shared.Models
{
    public class PriceDifferenceDto
    {
        private DateTimeOffset _startDate;
        private DateTimeOffset _endDate;

        public DateTimeOffset StartDate
        {
            get => _startDate.ToUniversalTime();
            set => _startDate = value.ToUniversalTime();
        }

        public DateTimeOffset EndDate
        {
            get => _endDate.ToUniversalTime();
            set => _endDate = value.ToUniversalTime();
        }

        public decimal PriceDifference { get; set; }
    }
}
