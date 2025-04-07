using FuturesPrice.Shared.Models;

namespace FuturesPrice.DAL.Interfaces
{
    public interface ILogRepository
    {
        Task SaveLogAsync(LogEntry log);
    }
}
