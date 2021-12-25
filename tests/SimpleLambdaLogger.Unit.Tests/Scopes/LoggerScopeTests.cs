using System;
using System.Linq;
using FluentAssertions;
using Moq;
using SimpleLambdaLogger.Unit.Tests.Data;
using Xunit;

namespace SimpleLambdaLogger.Unit.Tests.Scopes
{
    public class LoggerScopeTests
    {
        [Theory,AutoMoqData]
        internal void Log_WithLogEvent_ShouldAddLogToLogsCollection(
            DefaultScope sut,
            string scopeName,
            string? contextId,
            LogEventLevel scopeLogLevel,
            BaseScope parentScope,
            LogEvent logEvent
        )
        {
            sut.Log(LogEventLevel.Critical, new Exception("exception"), "error message");
            sut.Logs.Count.Should().Be(1);
        }
        
        [Theory,AutoMoqData]
        internal void Log_WithLogEvents_ShouldAddLogToLogsCollection(
            DefaultScope sut,
            string scopeName,
            string? contextId,
            LogEventLevel scopeLogLevel,
            BaseScope parentScope,
            LogEvent[] logEvents
        )
        {
            var count = logEvents.Length;
            for (int i = 0; i < count; i++)
            {
                var logEvent = logEvents[i];
                sut.Log(LogEventLevel.Critical, new Exception("exception"), "error message");    
            }
            
            sut.Logs.Count.Should().Be(count);
        }
        
        [Theory,AutoMoqData]
        internal void Log_WithLogEvents_ShouldFormatAndLogMessage(
            DefaultScope sut,
            string scopeName,
            string? contextId,
            LogEventLevel scopeLogLevel,
            BaseScope parentScope
        )
        {
            var messageTemplate = "Message with 3 parameters: {0}, {1} and {2}";
            var arguments = new object[3] {"parameter 1", "parameter 2", "parameter 3" };

            sut.Log(LogEventLevel.Information, messageTemplate, arguments);

            sut.Logs.Count.Should().Be(1);
            var actual = sut.Logs.FirstOrDefault();
            actual.Should().NotBeNull();
            actual.MessageTemplate.Should().Be(messageTemplate);
        }
        
        [Theory,AutoMoqData]
        public void Dispose_WithParentScope_ShouldSetCurrentScope(
            string scopeName,
            string? contextId,
            LogEventLevel scopeLogLevel
            )
        {
            var parentScope = new Mock<BaseScope>();
            var sut = new DefaultScope(scopeName, contextId, scopeLogLevel, parentScope.Object);
            sut.Dispose();

            SimpleLogger.CurrentScope.Value.Should().Be(parentScope.Object);
        }
        
        [Theory,AutoMoqData]
        public void Dispose_WithoutParentScope_ShouldSetCurrentScopeAsNull(
            string scopeName,
            string? contextId,
            LogEventLevel scopeLogLevel
            )
        {
            var sut = new DefaultScope(scopeName, contextId, scopeLogLevel, null);
            sut.Dispose();
        
            SimpleLogger.CurrentScope.Value.Should().BeNull();
        }
    }
}