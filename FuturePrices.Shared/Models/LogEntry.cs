using System.ComponentModel.DataAnnotations;

namespace FuturesPrice.Shared.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public required string Level { get; set; }
        public required string Message { get; set; }
        public string Exception { get; set; }
    }
}
