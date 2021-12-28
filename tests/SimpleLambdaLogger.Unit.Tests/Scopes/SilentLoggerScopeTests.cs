using System;
using FluentAssertions;
using Moq;
using SimpleLambdaLogger.Events;
using SimpleLambdaLogger.Scopes;
using SimpleLambdaLogger.Unit.Tests.Data;
using Xunit;

namespace SimpleLambdaLogger.Unit.Tests.Scopes
{
    public class SilentLoggerScopeTests
    {
        private readonly SilentScope _sut;

        public SilentLoggerScopeTests()
        {
            Mock<SilentScope> parentScope = new Mock<SilentScope>();
            _sut = new SilentScope(parentScope.Object);
        }

        [Theory]
        [InlineData(LogEventLevel.Critical, "log message", null)]
        [InlineData(LogEventLevel.Debug, "", null)]
        [InlineData(LogEventLevel.Information, null, "", "")]
        [InlineData(LogEventLevel.Information, " ", " ", "")]
        public void Log_ShouldNotThrow(LogEventLevel level, string message, params object[] args)
        {
            _sut.Log(level, message, args);
        }
        
        [Theory]
        [ClassData(typeof(SilentLoggerTestData))]
        public void Log_WithException_ShouldNotThrow(LogEventLevel level, Exception exception, string message, params object[] args)
        {
            _sut.Log(level, message, args);
        }
    }
}