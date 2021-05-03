using System;
using System.Threading.Tasks;

namespace StackLog
{
    public interface IStackLog
    {
        Task LogFatal(string message);
        Task LogFatal(Exception es);
        Task LogInformation(string message);
        Task LogDebug(string message);
        Task LogWarning(string message);
        Task LogCloudWatch(StackLoggerActionMethodInformation action);
        Task LogError(string message);
    }
}