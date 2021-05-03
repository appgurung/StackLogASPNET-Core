namespace StackLog.Configuration
{
    public class StackLogType
    {
        public const string StackInformation = "__STACKINFO__";
        public const string StackDebug = "__STACKDEBUG__";
        public const string StackWarn = "__STACKWARN__";
        public const string StackFatal = "__STACKFATAL__";
        public const string StackError = "__STACKERROR__";

       
    }

    public class StackLogTypeCode
    {
        public const int StackInfoCode = 1;
        public const int StackWarnCode = 2;
        public const int StackDebugCode = 3;
        public const int StackErrorCode = 4;
        public const int StackFatalCode = 5;
    }
}