using System;
using FluentAssertions;
using Moq;
using SimpleLambdaLogger.Unit.Tests.Data;
using Xunit;

namespace SimpleLambdaLogger.Unit.Tests.Scopes
{
    public class SilentLoggerScopeTests
    {
        private readonly SilentLoggerScope _sut;

        public SilentLoggerScopeTests()
        {
            Mock<SilentLoggerScope> parentScope = new Mock<SilentLoggerScope>();
            _sut = new SilentLoggerScope(parentScope.Object);
        }

        [Fact]
        public void Dispose_WithParentScope_ShouldSetCurrentScope()
        {
            var parentScope = new Mock<BaseLoggerScope>();
            SilentLoggerScope sut = new SilentLoggerScope(parentScope.Object);
            sut.Dispose();

            SimpleLogger.CurrentScope.Value.Should().Be(parentScope.Object);
        }
        
        [Fact]
        public void Dispose_WithoutParentScope_ShouldSetCurrentScopeAsNull()
        {
            SilentLoggerScope sut = new SilentLoggerScope(null);
            sut.Dispose();

            SimpleLogger.CurrentScope.Value.Should().BeNull();
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