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
using System.Runtime.InteropServices;

namespace StackLog
{
    public delegate void InitializeStackLog(string logType, StackLogRequest loggerRequest);
    public interface IStackLog
    {
        Task<IStackLog> LogFatal(string message, [Optional] string x);

        Task<IStackLog> LogFatal(Exception es, [Optional] string x);
        Task<IStackLog> Info(string message, [Optional] string x);
        Task<IStackLog> LogDebug(string message, [Optional] string x);
        Task<IStackLog> LogWarning(string message, [Optional] string x);
        Task<IStackLog> LogCloudWatch(StackLogResponse logInformation, [Optional] string x);
        Task<IStackLog> LogError(string message, [Optional] string x);
        
        //Task<IStackLog> Format(string message, object? arg0);
        string bucketKey { get; }
        string secretKey { get; }
    }
}