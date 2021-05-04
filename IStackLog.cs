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

    public sealed class StackLog : IStackLog
    {
        protected void Log(InitializeStackLog log, string logType, StackLogRequest loggerRequest)
        {
           
            log.Invoke(logType, loggerRequest);
        }

        private StackLogOptions _options;

        private ILoggerService _log;
        public StackLog(StackLogOptions options)
        {
            _options = options;
            _log = new LoggerService(_options.bucketKey, _options.secretKey, _options.enableCloudWatch);
        }

        public void InitializeConfiguration(StackLogOptions options)
        {
            // apsettings.json
            // startup
            //
            // 
        }
        public void DoExecuteLog(InitializeStackLog log, string logType, StackLogRequest loggerRequest)
        {
            Log(log, logType, loggerRequest);
        }
        
        public void CreateLog(string logType, StackLogRequest loggerRequest)
        {
            
            switch(logType)
            {
                case StackLogType.StackInformation:
                    loggerRequest.logTypeId = StackLogTypeCode.StackInfoCode;
                    _log.Log(loggerRequest);
                    break;
                case StackLogType.StackFatal:
                    _log.Log(loggerRequest);
                    break;
                case StackLogType.StackDebug:
                    _log.Log(loggerRequest);
                    break;
                case StackLogType.StackWarn:
                    _log.Log(loggerRequest);
                    break;
                case StackLogType.StackError:
                    _log.Log(loggerRequest);
                    break;
            
            }
            
            _log.Log(loggerRequest);
        }
        public Task LogFatal(string message)
        {
            var log = new InitializeStackLog(CreateLog);
            
            DoExecuteLog(log, StackLogType.StackFatal, new StackLogRequest()
            {
                logMessage =  message
            });
            
            throw new NotImplementedException();
        }

        public Task LogFatal(Exception es)
        {
            throw new NotImplementedException();
        }

        public async Task LogInformation(string message)
        {
            _log.Log(new StackLogRequest()
            {
                logTypeId = StackLogTypeCode.StackInfoCode,
                logMessage = message
            });
        }

        public async Task LogDebug(string message)
        {
            _log.Log(new StackLogRequest()
            {
                logTypeId = StackLogTypeCode.StackDebugCode,
                logMessage = message
            });
        }

        public async Task LogWarning(string message)
        {
            _log.Log(new StackLogRequest()
            {
                logTypeId = StackLogTypeCode.StackWarnCode,
                logMessage = message
            });
        }

        public async Task LogCloudWatch(StackLogExceptionInformation logInformation)
        {
            _log.LogCloudWatch(null);
        }

        public async Task LogError(string message)
        {
            _log.Log(new StackLogRequest()
            {
                logTypeId = StackLogTypeCode.StackErrorCode,
                logMessage = message
            });
        }
    }
}