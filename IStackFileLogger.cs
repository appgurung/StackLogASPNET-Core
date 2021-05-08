using System.Threading.Tasks;
using StackLog.Configuration;

namespace StackLog
{
    public class IStackFileLogger : StackLogBaseExtension
    {
        private IStackLog _logger;
        public IStackFileLogger(IStackLog logger) : base(logger)
        {
            _logger = logger;
        }

        private Task LogToFile(StackLogRequest request, string logType)
        {
            string logTypeMessage = GetLogType(logType);
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