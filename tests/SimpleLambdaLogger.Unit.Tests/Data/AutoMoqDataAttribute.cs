using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace SimpleLambdaLogger.Unit.Tests.Data
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(() => new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}