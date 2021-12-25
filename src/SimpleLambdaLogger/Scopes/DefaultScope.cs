using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Web;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace SimpleLambdaLogger
{
    internal class DefaultScope : BaseScope
    {
        private static Dictionary<LogEventLevel, string> LogLevelsLookup = new()
        {
            {LogEventLevel.Trace, "Trace"},
            {LogEventLevel.Debug, "Debug"},
            {LogEventLevel.Information, "Information"},
            {LogEventLevel.Warning, "Warning"},
            {LogEventLevel.Error, "Error"},
            {LogEventLevel.Critical, "Critical"}
        };
        
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        private readonly LogEventLevel _scopeLogLevel;

        private bool WriteLogs => _maxLogLevel >= _scopeLogLevel || ChildScopes.Any(childScope => childScope.WriteLogs);

        private LogEventLevel _maxLogLevel;

        public string Name { get; protected set; }

        public string? ContextId { get; protected set; }

        public ICollection<LogEvent> Logs { get; } = new List<LogEvent>();

        public long Duration => _stopwatch.ElapsedMilliseconds;

        public DefaultScope(
            string scopeName,
            string? contextId,
            LogEventLevel scopeLogLevel,
            BaseScope parentScope)
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

            var builder = new StringBuilder();
            CreateLog(builder, this);
            var result = builder.ToString();
            Console.WriteLine(result);

            SimpleLogger.CurrentScope.Value = null; // memory leaks without this line.
        }

        private StringBuilder CreateLog(StringBuilder builder, DefaultScope scope)
        {
            builder.AppendFormat("{{\"scope\": \"{0}\",", scope.Name);
            builder.AppendFormat("\"duration\": {0}", scope.Duration.ToString());

            if (!string.IsNullOrEmpty(scope.ContextId))
            {
                builder.AppendFormat(",\"contextId\": \"{0}\"", scope.ContextId);
            }

            if (scope.Logs.Count > 0)
            {
                builder.Append(",\"logs\": [");
                for (var i = 0; i < scope.Logs.Count; i++)
                {
                    var log = scope.Logs.ElementAt(i);
                    builder.AppendFormat("{{\"level\": \"{0}\",", LogLevelsLookup[log.LogEventLevel]);
                    builder.AppendFormat("\"created\": \"{0}\"", log.Timestamp.ToString());

                    if (!string.IsNullOrEmpty(log.Message))
                    {
                        builder.AppendFormat(",\"message\": \"{0}\"", log.Message);
                    }

                    if (log.Exception != null)
                    {
                        builder.AppendFormat(",\"exception\": \"{0}\"", HttpUtility.JavaScriptStringEncode(log.Exception.ToString()));
                    }
                    builder.Append("}");
                    if (i < scope.Logs.Count - 1)
                    {
                        builder.Append(",");
                    }
                }

                builder.Append("]");
            }

            if (scope.ChildScopes.Any())
            {
                builder.Append(",\"childScopes\":[");
            
                for (int i = 0; i < scope.ChildScopes.Count; i++)
                {
                    var currentChildScope = scope.ChildScopes.ElementAt(i);
                    CreateLog(builder, currentChildScope);
                    if (i < scope.ChildScopes.Count - 1)
                    {
                        builder.Append(",");
                    }
                }
                
                builder.Append("]");
            }
            builder.Append("}");
            return builder;
        }
    }
}

