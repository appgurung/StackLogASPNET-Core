namespace StackLog.Configuration
{
    public class StackLogExceptionInformation
    {
        public string Message { get; set; }
        public int LineNumber { get; set; }
        public string FileName { get; set; }
        public string MethodName { get; set; }
        public string MethodParameters { get; set; }
        public string CodeSnippet { get; set; }
      
    }
}