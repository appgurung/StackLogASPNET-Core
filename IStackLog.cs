using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
                    loggerRequest.logTypeId = StackLogTypeCode.StackFatalCode;
                    _log.Log(loggerRequest);
                    break;
                case StackLogType.StackDebug:
                    loggerRequest.logTypeId = StackLogTypeCode.StackDebugCode;
                    _log.Log(loggerRequest);
                    break;
                case StackLogType.StackWarn:
                    loggerRequest.logTypeId = StackLogTypeCode.StackWarnCode;
                    _log.Log(loggerRequest);
                    break;
                case StackLogType.StackError:
                    loggerRequest.logTypeId = StackLogTypeCode.StackErrorCode;
                    _log.Log(loggerRequest);
                    break;
            
            }
            
          //  _log.Log(loggerRequest);
        }
        public Task LogFatal(string message)
        {
            return LogToLoggerService(LogDelegateProp, StackLogType.StackFatal, new StackLogRequest()
            {
                logMessage = message
            });
        }

        public Task LogFatal(Exception es)
        {
            //new StackTrace()
             var stackTrace = new StackTrace(es, true).GetFrame(0);
            var exceptionObject = new StackLogExceptionInformation()
            {
                MethodName = stackTrace.GetMethod().Name,
                FileName = stackTrace.GetFileName(),
                LineNumber =stackTrace.GetFileLineNumber(),
                //ethodParamter = ""
            };

            var methodParams = stackTrace.GetMethod().GetParameters().ToList();
            StringBuilder methodP = new StringBuilder();

            if (methodParams != null)
            {
                if (methodParams.Count > 0)
                {
                    foreach (var param in methodParams)
                    {
                        var currentParam = $"ParameterName:{param.Name},ParamterType:{param.ParameterType.Name}, " +
                            $"ParameterPosition:{param.Position}, ParamaterValue";
                        methodP.AppendLine(currentParam);
                        
                    }
                }
            }
            //exceptionObject.MethodParamter = methodP.ToString();

            var allLines = File.ReadLines(exceptionObject.FileName);
            var takenSome = allLines.Skip(Math.Abs(exceptionObject.LineNumber - 5)).Take(5).ToList();
            StringBuilder codeSnippet = new StringBuilder();
            int index = 0;
            foreach(string current in takenSome)
            { 
                var currentLine ="";
                int currentIndex = takenSome.IndexOf(current);

                if(currentIndex == 0)
                {
                    currentLine = current + "";
                }
                else
                {
                    if (index != (takenSome.Count - 1))
                    {
                        if (current == "" || current == "\n")
                        {
                            currentLine = "";
                        }
                        else
                        {
                            currentLine = current + "#";
                        }

                    }
                    else
                    {
                        currentLine = current + "";
                    }
                }
                

                codeSnippet.Append(currentLine);
                index++;
            }
            var log = new InitializeStackLog(CreateLog);
            

            string realFileName = Path.GetFileName(exceptionObject.FileName ?? "") ?? "";
            var request = new StackLogRequest()
            {
                 fileLineNumber = exceptionObject.LineNumber,
                 codeSnippet = codeSnippet.ToString(),
                 fileName = realFileName,
                 logMessage = es.Message,
                 logTypeId = StackLogTypeCode.StackFatalCode,
                 methodName = exceptionObject.MethodName,
                 methodParam = methodP.ToString()
            };

            return LogToLoggerService(LogDelegateProp, StackLogType.StackFatal,request);
        }

        public Task LogInformation(string message)
        {
            return LogToLoggerService(LogDelegateProp, StackLogType.StackInformation, new StackLogRequest()
            {
                logMessage = message
            });
        }

        public Task LogDebug(string message)
        {
            return LogToLoggerService(LogDelegateProp, StackLogType.StackDebug, new StackLogRequest()
            {
                logMessage = message
            });
        }

        public Task LogWarning(string message)
        {
            return LogToLoggerService(LogDelegateProp, StackLogType.StackWarn, new StackLogRequest()
            {
                logMessage = message
            });
        }

        public async Task LogCloudWatch(StackLogExceptionInformation logInformation)
        {
            _log.LogCloudWatch(null);
        }

        public  Task LogError(string message)
        {
            return LogToLoggerService(LogDelegateProp, StackLogType.StackError, new StackLogRequest()
            {
                logMessage = message
            });
        }
        
        private InitializeStackLog LogDelegateProp => new InitializeStackLog(CreateLog);

        private Task LogToLoggerService(InitializeStackLog log, string type, StackLogRequest request)
        {
           // var log = new InitializeStackLog(CreateLog);
            
            DoExecuteLog(log,type , request);

            return Task.CompletedTask;
        }
    }
}