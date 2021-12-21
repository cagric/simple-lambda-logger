using System;

namespace SimpleLambdaLogger
{
    public interface ILoggerScope : IDisposable
    {
        void Log(LogEventLevel logEventLevel, string message, params object[] args);

        void Log(LogEventLevel logEventLevel, Exception exception, string message, params object[] args);
    }
}