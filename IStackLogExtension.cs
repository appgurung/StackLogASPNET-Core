using System;
using System.Threading.Tasks;
using StackLog.Configuration;

namespace StackLog
{
    public interface IStackLogExtension
    {
        // StackLogBaseExtension Fatal { get;  }

        StackLogExtension Info(string message);// { get; }
        StackLogExtension Debug(string message);
        StackLogExtension Warning(string message);
        StackLogExtension CloudWatch(StackLogResponse logInformation);
       
       // StackLogExtension LogFatal(Exception es);

    }

    public interface IStackLogFatalExtension
    {
        StackLogFatalExtension Fatal(Exception es);
    }

    
}
