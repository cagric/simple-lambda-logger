using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
namespace SimpleLambdaLogger
{
    internal class LogEvent
    {
        public LogEvent(
            LogEventLevel logEventLevel,
            DateTimeOffset timestamp,
            string message,
            Exception exception)
        {
            LogEventLevel = logEventLevel;
            Timestamp = timestamp;
            Message = message;
            Exception = exception;
        }
        
        public LogEventLevel LogEventLevel { get; }

        public string Message { get; }

        public DateTimeOffset Timestamp { get; }

        public Exception Exception { get; }
    }
}