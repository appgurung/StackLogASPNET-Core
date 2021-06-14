using System;
using System.IO;
using System.Threading.Tasks;
using StackLog.Configuration;

namespace StackLog
{
    public class IStackFileLogger : StackLogBaseExtension
    {
        static WeakReference<object> infoWeakSyncObjectReference;
        static WeakReference<object> warningWeakSyncObjectReference;
        static WeakReference<object> errorWeakSyncObjectReference;


        static object InfoSyncObj
        {
            get
            {
                if (infoWeakSyncObjectReference != null && infoWeakSyncObjectReference.TryGetTarget(out object syncObj))
                {
                    return syncObj;
                }

                infoWeakSyncObjectReference = new WeakReference<object>(new object(), false);
                infoWeakSyncObjectReference.TryGetTarget(out syncObj);

                return syncObj;
            }
        }

        static object WarningSyncObj
        {
            get
            {
                if (warningWeakSyncObjectReference != null && warningWeakSyncObjectReference.TryGetTarget(out object syncObj))
                {
                    return syncObj;
                }

                warningWeakSyncObjectReference = new WeakReference<object>(new object(), false);
                warningWeakSyncObjectReference.TryGetTarget(out syncObj);

                return syncObj;
            }
        }

        static object ErrorSyncObj
        {
            get
            {
                if (errorWeakSyncObjectReference != null && errorWeakSyncObjectReference.TryGetTarget(out object syncObj))
                {
                    return syncObj;
                }

                errorWeakSyncObjectReference = new WeakReference<object>(new object(), false);
                errorWeakSyncObjectReference.TryGetTarget(out syncObj);

                return syncObj;
            }
        }
        private IStackLog _logger;
        public IStackFileLogger(IStackLog logger) : base(logger)
        {
            _logger = logger;
        }

        public  Task LogInfo(string message, string logType, string path)
        {
           // base.LogInformation(message);
           return LogToFile(new StackLogRequest(){logMessage = message}, logType, path, logType);
        }

        public Task LogInfo(StackLogRequest req, string logType, string path)
        {
            return LogToFile(req, logType, path, logType);
        }
       
        private Task LogToFile(StackLogRequest request, string logType, string path="", string filename="")
        {

            
            string logTypeMessage = GetLogType(logType);
            string LogDir = path;
            if (String.IsNullOrEmpty(LogDir))
            {
                // y -> mon -> day of month -> type

                LogDir = Path.Combine(Directory.GetCurrentDirectory(), "StackLogs",
                        DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MMM"), DateTime.Now.ToString("ddMMMyyy"),
                        logType);
                //LogDir = Path.Combine(Directory.GetCurrentDirectory(), "StackLogs", logType, 
                //    DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MMM"), DateTime.Now.ToString("ddMMMyyy"));
            }

            if(!String.IsNullOrEmpty(path))
            {
                LogDir = Path.Combine(path, "StackLogs",
                        DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MMM"), DateTime.Now.ToString("ddMMMyyy"),
                        logType);
            }

            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }
            

            string logFileName = $"{filename}_{DateTime.Today.ToString("ddMMMyyyy")}.stlog";

            lock (InfoSyncObj)
            {
                WriteLogToFile(request.logMessage, Path.Combine(LogDir, logFileName), logTypeMessage);
            }

            return Task.CompletedTask;
        }
        
        public static void WriteLogToFile(string message, string logFilePath, string logType = StackLogType.StackInformation)
        {
            File.AppendAllText(logFilePath, "\r\n");
            File.AppendAllText(logFilePath, $"[Event Time::{DateTime.Now:hh':'mm':'ss}{logType}{message}]");
            File.AppendAllText(logFilePath, "\r\n");
           // File.AppendAllText(logFilePath, $"Event Time: {DateTime.Now:hh':'mm':'ss} | {message}\r\n\r\n");
           // File.AppendAllText(logFilePath, $"------------------------------------------------------------------------------------\r\n\r\n");
        }

        private string GetLogType(string logType)
        {
            if (logType == StackLogType.StackDebug)
                return "::DEBUG::";
            
            if (logType == StackLogType.StackWarn)
                return "::WARNING::";
            
            if (logType == StackLogType.StackFatal)
                return "::FATAL::";
            
            if (logType == StackLogType.StackError)
                return "::ERROR::";

            return "::INFORMATION::";
        }
    }
}