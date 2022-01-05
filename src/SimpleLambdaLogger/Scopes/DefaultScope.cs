using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using SimpleLambdaLogger.Events;
using SimpleLambdaLogger.Internal;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace SimpleLambdaLogger.Scopes
{
    internal class DefaultScope : BaseScope
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private readonly LogEventLevel _scopeLogLevel;
        private readonly LogEventLevel _minFailureLogLevel;
        private bool WriteLogs => _maxLogLevel >= _scopeLogLevel || ChildScopes.Any(childScope => childScope.WriteLogs);
        
        private LogEventLevel _maxLogLevel;

        public bool Success => !(_maxLogLevel >=_minFailureLogLevel  || ChildScopes.Any(childScope => !childScope.Success));
        
        public string Name { get; protected set; }

        public string? ContextId { get; protected set; }

        public ICollection<LogEvent> Logs { get; } = new List<LogEvent>();

        public long Duration => _stopwatch.ElapsedMilliseconds;

        public DefaultScope(
            string scopeName,
            string? contextId,
            LogEventLevel scopeLogLevel,
            LogEventLevel minFailureLogLevel,
            BaseScope parentScope)
        {
            Name = scopeName;
            ContextId = contextId;
            _scopeLogLevel = scopeLogLevel;
            _minFailureLogLevel = minFailureLogLevel;

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
            var logEvent = new LogEvent(logEventLevel, DateTimeOffset.Now, message, exception, args);
            Logs.Add(logEvent);
        }

        public override void Dispose()
        {
            _stopwatch.Stop();

            if (ParentScope != null)
            {
                LoggingContext.ChangeCurrentScope(ParentScope);
                return;
            }

            if (!WriteLogs)
            {
                return;
            }

            var logMessage = JsonSerializer.Serialize(this, Settings.SerializationOptions);
            Console.WriteLine(logMessage);

            LoggingContext.ResetCurrentScope();
        }
    }
}

