using System.Runtime.CompilerServices;
using SimpleLambdaLogger.Scopes;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace SimpleLambdaLogger.Formatters
{
    internal interface ILogFormatter
    {
        string For(DefaultScope scope);
    }
}