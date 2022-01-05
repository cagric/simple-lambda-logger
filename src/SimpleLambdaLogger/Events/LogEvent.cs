using System;
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
            Level = logEventLevel;
            Timestamp = timestamp;
            Exception = exception?.ToString();
            if (!string.IsNullOrEmpty(messageTemplate) && args != null && args.Length > 0)
            {
                Message = string.Format(messageTemplate, args);
            }
            else
            {
                Message = messageTemplate;
            }
        }

        public LogEventLevel Level { get; }

        public string Message { get; }

        public DateTimeOffset Timestamp { get; }

        public string Exception { get; }
    }
}