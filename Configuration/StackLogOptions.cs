namespace StackLog.Configuration
{
    public class StackLogOptions
    {
       
        public string secretKey { get; set; }
        public string bucketKey { get; set; }
        public bool enableCloudWatch { get; set; } = true;
        public bool enableFileLogging { get; set; } = false;
        public bool enableConsoleLogging { get; set; } = false;
        public string filePath { get; set; }
        public string errorViewName { get; set; } = "Error";
    }
}