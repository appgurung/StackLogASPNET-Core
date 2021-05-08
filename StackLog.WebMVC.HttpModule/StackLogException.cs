using System;

namespace StackLog.WebMVC.HttpModule
{
    public class StackLogException : Exception
    {
        public StackLogException(string message, Exception innerException):base(message, innerException)
        {
                
        }

        public StackLogException(string message):base(message)
        { }
    }
}