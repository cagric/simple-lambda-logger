using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
namespace SimpleLambdaLogger
{
    internal abstract class BaseScope : ILoggerScope
    {
        protected BaseScope ParentScope;

        public ICollection<DefaultScope> ChildScopes { get; } = new List<DefaultScope>();
        
        public abstract void Log(LogEventLevel logEventLevel, string message, params object[] args);

        public abstract void Log(LogEventLevel logEventLevel, Exception exception, string message, params object[] args);

        public abstract void Dispose();
    }
}