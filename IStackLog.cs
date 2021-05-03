using System;
using System.Threading.Tasks;
using StackLog.Configuration;

namespace StackLog
{
    public delegate void InitializeStackLog(string logType, StackLogRequest loggerRequest);
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

    public class StackLog : IStackLog
    {
        public Task LogFatal(string message)
        {
            throw new NotImplementedException();
        }

        public Task LogFatal(Exception es)
        {
            throw new NotImplementedException();
        }

        public Task LogInformation(string message)
        {
            throw new NotImplementedException();
        }

        public Task LogDebug(string message)
        {
            throw new NotImplementedException();
        }

        public Task LogWarning(string message)
        {
            throw new NotImplementedException();
        }

        public Task LogCloudWatch(StackLogExceptionInformation logInformation)
        {
            throw new NotImplementedException();
        }

        public Task LogError(string message)
        {
            throw new NotImplementedException();
        }
    }
}