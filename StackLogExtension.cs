using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using StackLog.Configuration;

namespace StackLog
{
    public class StackLogExtension
    {
        ILoggerService _log;
        string _logType;
        private StackLogRequest loggerRequest;
        readonly string _message;
        readonly Exception _ex;
        readonly StackLogResponse stackLogResponse;
        public StackLogExtension(string logType, IStackLogOptions options, [Optional] string message)
        {
            _log = new LoggerService(options);
            _logType = logType;
            loggerRequest = new StackLogRequest();
            _message = message;
        }


        public StackLogExtension(string logType, IStackLogOptions options, Exception message)
        {
            _log = new LoggerService(options);
            _logType = logType;
            loggerRequest = new StackLogRequest();
            _ex = message;
        }


        public StackLogExtension(IStackLogOptions options, StackLogResponse message)
        {
            _log = new LoggerService(options);
           // _logType = logType;
            loggerRequest = new StackLogRequest();
            stackLogResponse = message;
        }
        // StackLogResponse

        public async Task To(string buckey)
        {
            switch (_logType)
            {
                case StackLogType.StackInformation:
                    loggerRequest.logTypeId = StackLogTypeCode.StackInfoCode;
                    loggerRequest.logMessage = _message;
                    _log.SetBucketKey(buckey);

                    await _log.Log(loggerRequest);
                    break;
                case StackLogType.StackFatal:
                    loggerRequest.logTypeId = StackLogTypeCode.StackFatalCode;
                    await _log.Log(loggerRequest);
                    break;
                case StackLogType.StackDebug:
                    loggerRequest.logTypeId = StackLogTypeCode.StackDebugCode;
                    await _log.Log(loggerRequest);
                    break;
                case StackLogType.StackWarn:
                    loggerRequest.logTypeId = StackLogTypeCode.StackWarnCode;
                    await _log.Log(loggerRequest);
                    break;
                case StackLogType.StackError:
                    loggerRequest.logTypeId = StackLogTypeCode.StackErrorCode;
                    await _log.Log(loggerRequest);
                    break;

            }
        }

        public async Task To(params string[] bucketKeys)
        {
            throw new NotImplementedException();
        }

    }

    public class StackLogFatalExtension
    {
        IStackLog stackLog;
        readonly Exception es;
        readonly string message;
        public StackLogFatalExtension(IStackLogOptions options, string message)
        {
            stackLog = new StackLog(options);
            this.message = message;
        }

        public StackLogFatalExtension(IStackLogOptions options, Exception message)
        {
            stackLog = new StackLog(options);
            this.es = message;
        }
    }
}
