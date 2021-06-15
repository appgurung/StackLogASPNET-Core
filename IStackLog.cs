using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using StackLog.Configuration;
using Newtonsoft.Json;

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
        Task LogCloudWatch(StackLogResponse logInformation);
        Task LogError(string message);
        string bucketKey { get; }
        string secretKey { get; }
    }

    public sealed class StackLog : IStackLog
    {
        protected void Log(InitializeStackLog log, string logType, StackLogRequest loggerRequest)
        {
           
            log.Invoke(logType, loggerRequest);
        }

        private IStackLogOptions _options;

        private ILoggerService _log;
        
        private void SetStackLogOptions(IStackLogOptions opts)
        {
            _options = opts;
        }
        public StackLog(IStackLogOptions options)
        {
            //TryReadConfiguration(out StackLogOptions result);

            //if (result != null && options == null)
            //{
            //    _options = result;
            //}
            //else
            //{
            //   
            //}
            _options = options;
            //options += SetStackLogOptions;

            _log = new LoggerService(options);
        }

        public void TryReadConfiguration(out StackLogOptions options)
        {
            StackLogOptions _opts = null;

            // var detectRunningPlatForm = RunningOnMvc();
            //
            // if (detectRunningPlatForm != null)
            // {
            //     _opts = detectRunningPlatForm;
            // }
            options = _opts;

        }

        private StackLogOptions RunningOnCore()
        {
            return default;
        }
        private StackLogOptions RunningOnMvc()
        {
            StackLogOptions opts = null;
            string secretKeyCode = StackLogConfiguration.ReadConfigSettings("StackLogSecretKey").ToString();
            string bucketKeyCode = StackLogConfiguration.ReadConfigSettings("StackLogBucketKey").ToString();
            string enableCloudWatch = StackLogConfiguration.ReadConfigSettings("enableCloudWatch").ToString();
            string enableFileLog = StackLogConfiguration.ReadConfigSettings("enableFileLog").ToString();
            string enableConsoleLogging = StackLogConfiguration.ReadConfigSettings("enableConsoleLogging").ToString();
            //IConfigurationBuilder build = IConfigurationBuilder.

            bool isCloudWatchEnabled = false;
            bool isFileLogEnabled = false;
            bool isConsoleLogEnabled = false;
            if (enableCloudWatch != null || enableCloudWatch != "")
            {
                isCloudWatchEnabled = enableCloudWatch == "true" || enableCloudWatch == "TRUE" || enableCloudWatch == "True" ? true : false;
            }
                
            if (enableFileLog != null || enableFileLog != "")
            {
                isFileLogEnabled = enableFileLog == "true" || enableFileLog == "TRUE" || enableFileLog == "True" ? true : false;
            }
                
            if (enableConsoleLogging != null || enableConsoleLogging != "")
            {
                isConsoleLogEnabled = enableFileLog == "true" || enableFileLog == "TRUE" || enableFileLog == "True" ? true : false;
            }

            if ((String.IsNullOrEmpty(secretKeyCode) || String.IsNullOrEmpty(bucketKeyCode)) && (!isFileLogEnabled || !isConsoleLogEnabled))
                return null;
            
            opts = new StackLogOptions()
            {
                secretKey = secretKeyCode,
                bucketKey = bucketKeyCode,
                enableCloudWatch = isCloudWatchEnabled,
                enableFileLogging = isFileLogEnabled,
                enableConsoleLogging = isConsoleLogEnabled
            };

            return opts;
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
        public async Task LogFatal(string message)
        {
            await DoConsoleLog(message, StackLogType.StackFatal);
            await DoFileLog(message, StackLogType.StackFatal);
            await LogToLoggerService(LogDelegateProp, StackLogType.StackFatal, new StackLogRequest()
            {
                logMessage = message
            });
        }

        private StackLogRequest CaptureFataInfo(Exception es)
        {
            //new StackTrace()
            var stackTrace = new StackTrace(es, true).GetFrame(0);
            //  var stackTrace = es.StackTrace;
            var exceptionObject = new StackLogExceptionInformation()
            {
                MethodName = stackTrace.GetMethod().Name,
                FileName = stackTrace.GetFileName(),
                LineNumber = stackTrace.GetFileLineNumber(),
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
            foreach (string current in takenSome)
            {
                var currentLine = "";
                int currentIndex = takenSome.IndexOf(current);

                if (currentIndex == 0)
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

            return request;
        }

        public async Task LogFatal(Exception es)
        {
            var request = CaptureFataInfo(es);

            await DoConsoleLog(es);
            DoFileLog(request, StackLogType.StackFatal);
            await LogToLoggerService(LogDelegateProp, StackLogType.StackFatal,request);
        }

        public async Task LogInformation(string message)
        {
            await DoConsoleLog(message, StackLogType.StackInformation);
            await DoFileLog(message, StackLogType.StackInformation);
            await LogToLoggerService(LogDelegateProp, StackLogType.StackInformation, new StackLogRequest()
            {
                logMessage = message
            });
        }

        public async Task LogDebug(string message)
        {
            await DoConsoleLog(message, StackLogType.StackDebug);
            await DoFileLog(message, StackLogType.StackDebug);
            await LogToLoggerService(LogDelegateProp, StackLogType.StackDebug, new StackLogRequest()
            {
                logMessage = message
            });
        }

        public async Task LogWarning(string message)
        {
            
            await DoConsoleLog(message, StackLogType.StackWarn);
           
            await DoFileLog(message, StackLogType.StackWarn);
            await LogToLoggerService(LogDelegateProp, StackLogType.StackWarn, new StackLogRequest()
            {
                logMessage = message
            });

            //return Task.CompletedTask;
        }

        private async Task DoConsoleLog(Exception es)
        {
            var request = JsonConvert.SerializeObject(CaptureFataInfo(es));

            await DoConsoleLog(request, StackLogType.StackFatal);
           // Console.ForegroundColor = ConsoleColor.Red;

        }
        private async Task DoConsoleLog(string message, string logType)
        {
            // 
            if (_options.enableConsoleLogging)
            {
                string msg = $"[EVENT TIME::{DateTime.Now:hh':'mm':'ss}::LEVEL::{logType}::MESSAGE::{message}]";
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
                Debug.WriteLine(msg);
                Console.ForegroundColor = ConsoleColor.White;
            }
           

           // return await Task.CompletedTask;
        }
        private async Task DoFileLog(string message, string logType)
        {
            //if (_options.enableFileLogging)
            //{
               
                if(_options.FileOptions != null)
                {
                    var fl = new IStackFileLogger(this);
                    var flOptions = _options.FileOptions;
                    if(flOptions.enable)
                    {
                        await fl.LogInfo(message, logType, flOptions.filePath, flOptions.fileName);
                    }
                    
                }
                
            //}
        }
        private async Task DoFileLog(StackLogRequest req, string logType)
        {
            //if (_options.enableFileLogging)
            //{
                
                if (_options.FileOptions != null)
                {
                    var fl = new IStackFileLogger(this);
                    var flOptions = _options.FileOptions;
                    if (flOptions.enable)
                    {
                        await fl.LogInfo(req, logType, flOptions.filePath, flOptions.fileName);
                    }

                }
          //  }
        }
        public async Task LogCloudWatch(StackLogResponse logInformation)
        {
           await _log.LogCloudWatch(logInformation);
        }

        public  Task LogError(string message)
        {
            DoFileLog(message, StackLogType.StackError);
            return LogToLoggerService(LogDelegateProp, StackLogType.StackError, new StackLogRequest()
            {
                logMessage = message
            });
        }
        
        private InitializeStackLog LogDelegateProp => new InitializeStackLog(CreateLog);

        public string ErrorViewName => _options != null
            ? _options.errorViewName
            : throw new StackLogException(StackLogExceptionErrors.NULL_OPTIONS);
        
        
        private string Secret_Key =>  _options != null
            ? _options.secretKey
            : throw new StackLogException(StackLogExceptionErrors.SECRET_KEY_MISSING);
        
        private string Bucket_Key =>  _options != null
            ? _options.bucketKey
            : throw new StackLogException(StackLogExceptionErrors.BUCKET_KEY_MISSING);

        public string bucketKey { get => Bucket_Key; }
        public string secretKey { get => Secret_Key;  }

        private async Task LogToLoggerService(InitializeStackLog log, string type, StackLogRequest request)
        {
           // var log = new InitializeStackLog(CreateLog);
            
            DoExecuteLog(log,type , request);

        }
    }
}