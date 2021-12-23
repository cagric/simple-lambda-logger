using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace SimpleLambdaLogger
{
    internal class LoggerScope : BaseLoggerScope
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        private readonly LogEventLevel _scopeLogLevel;

        private bool WriteLogs => _maxLogLevel >= _scopeLogLevel || ChildScopes.Any(childScope => childScope.WriteLogs);

        private LogEventLevel _maxLogLevel;

        public string Name { get; protected set; }

        public string? ContextId { get; protected set; }

        public ICollection<LogEvent> Logs { get; } = new List<LogEvent>();

        public long Duration => _stopwatch.ElapsedMilliseconds;

        public LoggerScope(
            string scopeName,
            string? contextId,
            LogEventLevel scopeLogLevel,
            BaseLoggerScope parentScope)
        {
            Name = scopeName;
            ContextId = contextId;
            _scopeLogLevel = scopeLogLevel;

            ParentScope = parentScope;
            ParentScope?.ChildScopes.Add(this);
        }

        public override void Log(LogEventLevel logEventLevel, string message, params object[] args)
        {
            Log(logEventLevel, null, message, args);
        }

        public override void Log(LogEventLevel logEventLevel, Exception exception, string message, params object[] args)
        {
            _maxLogLevel = logEventLevel > _maxLogLevel ? logEventLevel : _maxLogLevel;

            // todo: improve formatting
            if (args.Length > 0 && !string.IsNullOrEmpty(message))
            {
                message = string.Format(message, args);
            }

            var logEvent = new LogEvent(logEventLevel, DateTimeOffset.Now, message, exception);
            Logs.Add(logEvent);
        }

        public override void Dispose()
        {
            _stopwatch.Stop();

            if (ParentScope != null)
            {
                SimpleLogger.CurrentScope.Value = ParentScope;
                return;
            }

            if (!WriteLogs)
            {
                return;
            }

            var result = JsonSerializer.Serialize(this, SimpleLogger.SerializationOptions);
            Console.WriteLine(result);
            SimpleLogger.CurrentScope.Value = null; // memory leaks without this line.
        }
    }
}