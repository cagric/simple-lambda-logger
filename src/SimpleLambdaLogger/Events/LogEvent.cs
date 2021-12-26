using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]

namespace SimpleLambdaLogger.Events
{
    internal class LogEvent
    {
        public LogEvent(
            LogEventLevel logEventLevel,
            DateTimeOffset timestamp,
            string messageTemplate,
            Exception exception,
            params object[] args)
        {
            LogEventLevel = logEventLevel;
            Timestamp = timestamp;
            MessageTemplate = messageTemplate;
            Exception = exception;
            Args = args;
        }

        public LogEventLevel LogEventLevel { get; }

        public string MessageTemplate { get; }

        public DateTimeOffset Timestamp { get; }

        public Exception Exception { get; }
        
        public object[] Args { get; }
    }
}