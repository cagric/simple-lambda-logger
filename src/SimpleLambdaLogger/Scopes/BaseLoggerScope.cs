using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
namespace SimpleLambdaLogger
{
    internal abstract class BaseLoggerScope : ILoggerScope
    {
        protected BaseLoggerScope ParentScope;

        public ICollection<LoggerScope> ChildScopes { get; } = new List<LoggerScope>();
        
        public abstract void Log(LogEventLevel logEventLevel, string message, params object[] args);

        public abstract void Log(LogEventLevel logEventLevel, Exception exception, string message, params object[] args);

        public abstract void Dispose();
    }
}