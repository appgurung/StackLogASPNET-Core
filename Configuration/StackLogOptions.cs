namespace StackLog.Configuration
{
    public interface IStackLogOptions
    {
        public string secretKey { get; set; }
        public string bucketKey { get; set; }
        public bool enableCloudWatch { get; set; }
        public bool enableFileLogging { get; set; }
        public bool enableConsoleLogging { get; set; }
        public string filePath { get; set; }
        public string errorViewName { get; set; }
    }
    public class StackLogOptions : IStackLogOptions
    {
        public string secretKey { get; set; } 
        public string bucketKey { get; set; }
        public bool enableCloudWatch { get; set; }
        public bool enableFileLogging { get; set; }
        public bool enableConsoleLogging { get; set; }
        public string filePath { get; set; }
        public string errorViewName { get; set; }
    }
}