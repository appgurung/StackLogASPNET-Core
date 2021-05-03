namespace StackLog.Configuration
{
    public class StackLogRequest
    {
        public int logTypeId { get; set; }
        public string stackKey { get; set; }
        public string logMessage { get; set; }
        public string fileName { get; set; }
        public string methodName { get; set; }
        public object methodParam { get; set; }
        public string codeSnippet { get; set; }
        public int fileLineNumber { get; set; }
    }
}