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
        readonly IStackFileLogger _fileLog;
        readonly IStackLogOptions _options;
        public StackLogExtension(string logType, IStackLogOptions options, [Optional] string message)
        {
            _log = new LoggerService(options);
            _logType = logType;
            loggerRequest = new StackLogRequest();
            _message = message;
            _options = options;
            _fileLog = new IStackFileLogger(default);
        }


        public StackLogExtension(string logType, IStackLogOptions options, Exception message)
        {
            _log = new LoggerService(options);
            _logType = logType;
            loggerRequest = new StackLogRequest();
            _ex = message;
            _options = options;
            _fileLog = new IStackFileLogger(default);
        }


        public StackLogExtension(IStackLogOptions options, StackLogResponse message)
        {
            _log = new LoggerService(options);
           // _logType = logType;
            loggerRequest = new StackLogRequest();
            stackLogResponse = message;
            _options = options;
            _fileLog = new IStackFileLogger(default);
        }

        private async Task DoConsoleLog(string message, string logType)
        {
            // 
            if (_options.enableConsoleLogging)
            {
                string msg = $"[EVENT TIME:{DateTime.Now:hh':'mm':'ss} | LEVEL: {logType} | MESSAGE:{message}]";
                ConsoleColor currentColor = Console.ForegroundColor;
                if (logType == StackLogType.StackFatal)
                {
                    //var formerColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (logType == StackLogType.StackWarn)
                {
                    // var formerColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Console.WriteLine(msg);
                System.Diagnostics.Debug.WriteLine(msg);
                Console.ForegroundColor = currentColor;
            }


            // return await Task.CompletedTask;
        }

        private async Task FileLog(string message, string logType)
        {
            //if (_options.enableFileLogging)
            //{

            if (_options.FileOptions != null)
            {
                //var fl = new IStackFileLogger(this);
                var flOptions = _options.FileOptions;
                if (flOptions.enable)
                {
                    await _fileLog.LogInfo(message, logType, flOptions.filePath, flOptions.fileName);
                }

            }
            //  }
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
                    await FileLog(_message, StackLogType.StackInformation);
                    await DoConsoleLog(_message, StackLogType.StackInformation);
                    break;
                case StackLogType.StackFatal:
                    loggerRequest.logTypeId = StackLogTypeCode.StackFatalCode;

                    await _log.Log(loggerRequest);
                    await FileLog(_message, StackLogType.StackFatal);
                    await DoConsoleLog(_message, StackLogType.StackFatal);
                    break;
                case StackLogType.StackDebug:
                    loggerRequest.logTypeId = StackLogTypeCode.StackDebugCode;

                    await _log.Log(loggerRequest);
                    await FileLog(_message, StackLogType.StackDebug);
                    await DoConsoleLog(_message, StackLogType.StackDebug);
                    break;
                case StackLogType.StackWarn:
                    loggerRequest.logTypeId = StackLogTypeCode.StackWarnCode;

                    await _log.Log(loggerRequest);
                    await FileLog(_message, StackLogType.StackWarn);
                    await DoConsoleLog(_message, StackLogType.StackWarn);
                    break;
                case StackLogType.StackError:
                    loggerRequest.logTypeId = StackLogTypeCode.StackErrorCode;

                    await _log.Log(loggerRequest);
                    await FileLog(_message, StackLogType.StackError);
                    await DoConsoleLog(_message, StackLogType.StackError);
                    break;

            }
        }

        public async Task To(params string[] bucketKeys)
        {
            foreach (var item in bucketKeys)
            {
                await To(item);
            }
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
