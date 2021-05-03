using System;
using System.Threading.Tasks;
using StackLog.Configuration;

namespace StackLog
{
    public interface IStackLog
    {
        Task LogFatal(string message);
        Task LogFatal(Exception es);
        Task LogInformation(string message);
        Task LogDebug(string message);
        Task LogWarning(string message);
        Task LogCloudWatch(StackLogExceptionInformation logInformation);
        Task LogError(string message);
    }
}