using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using SimpleLambdaLogger.Internal;
using SimpleLambdaLogger.Scopes;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace SimpleLambdaLogger.Formatters
{
    internal class JsonFormatter: ILogFormatter
    {
        public string For(DefaultScope scope)
        {
            var builder = new StringBuilder();
            CreateLog(builder, scope);
            var result = builder.ToString();
            return result;
        }
        
        private void CreateLog(StringBuilder builder, DefaultScope scope, string? parentScopeName = null)
        {
            var fullScopeName = !string.IsNullOrEmpty(parentScopeName) ? $"{parentScopeName}.{scope.Name}" : scope.Name;
            builder.AppendFormat("{{\"scope\": \"{0}\",", fullScopeName);
            builder.AppendFormat("\"duration\": {0},", scope.Duration.ToString());
            builder.AppendFormat("\"success\": {0}", scope.Success.ToString().ToLower());
            
            
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
                    CreateLog(builder, currentChildScope, fullScopeName);
                    if (i < scope.ChildScopes.Count - 1)
                    {
                        builder.Append(",");
                    }
                }
                
                builder.Append("]");
            }
            builder.Append("}");
        }
    }
}

    