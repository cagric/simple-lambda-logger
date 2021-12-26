using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using SimpleLambdaLogger.Internal;
using SimpleLambdaLogger.Scopes;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace SimpleLambdaLogger.Formatters
{
    internal class JsonLogFormatter: ILogFormatter
    {
        public string For(DefaultScope scope)
        {
            var builder = new StringBuilder();
            CreateLog(builder, scope);
            var result = builder.ToString();
            return result;
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
                    builder.AppendFormat("{{\"level\": \"{0}\",", Settings.LogLevelsLookup[log.LogEventLevel]);
                    builder.AppendFormat("\"created\": \"{0}\"", log.Timestamp.ToString());

                    if (!string.IsNullOrEmpty(log.MessageTemplate))
                    {
                        if (log.Args != null && log.Args.Length > 0)
                        {
                            builder.AppendFormat(",\"message\": \"{0}\"", string.Format(log.MessageTemplate, log.Args));   
                        }
                        else
                        {
                            builder.AppendFormat(",\"message\": \"{0}\"", log.MessageTemplate);   
                        }
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

    