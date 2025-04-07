using System.Threading.Tasks;

namespace FuturesPrice.BusinessLogic.Interfaces
{
    public interface ILoggingService
    {
        Task LogInfoAsync(string message);
        Task LogErrorAsync(string message, Exception ex = null);
        Task SaveLogAsync(string level, string message, string exception = null);
    }
}
