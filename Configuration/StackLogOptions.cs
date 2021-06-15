using System;

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
        public FileOptions FileOptions { get; set; }
        //public Action<FileOptions> fileOptions { get; set; }
        public string errorViewName { get; set; }
    }

    public class FileOptions
    {
        public bool enable { get; set; }
        public string filePath { get; set; }
        public string fileName { get; set; }
    }
    public class StackLogOptions : IStackLogOptions
    {
        FileOptions fl = new FileOptions();
        public string secretKey { get; set; } 
        public string bucketKey { get; set; }
        public bool enableCloudWatch { get; set; }
        public bool enableFileLogging { get; set; }
        public bool enableConsoleLogging { get; set; }
        public string filePath { get; set; }
        public string errorViewName { get; set; }
        public FileOptions FileOptions { get ; set ; }
        //public Action<FileOptions> fileOptions { get; set; }
        
       


        public StackLogOptions()
        {
           
        }
        //public Action<FileOptions> FileOptions { get ; set ; }
    }
}