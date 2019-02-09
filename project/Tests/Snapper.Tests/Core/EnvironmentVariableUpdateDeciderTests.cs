using System;
using Snapper.Core;
using Xunit;

namespace Snapper.Tests.Core
{
    public class EnvironmentVariableUpdateDeciderTests
    {
        private readonly string _envVar;
        private readonly EnvironmentVariableUpdateDecider _decider;

        public EnvironmentVariableUpdateDeciderTests()
        {
            _envVar = Guid.NewGuid().ToString();
            _decider = new EnvironmentVariableUpdateDecider(_envVar);
        }

        [Fact]
        public void NoEnvironmentVariable_ShouldNotUpdate()
        {
            Assert.False(_decider.ShouldUpdateSnapshot());
        }

        [Theory]
        [InlineData("true")]
        [InlineData("True")]
        [InlineData("TRUE")]
        [InlineData("tRuE")]
        public void TrueEnvironmentVariableSet_ShouldUpdate(string value)
        {
            Environment.SetEnvironmentVariable(_envVar, value);
            Assert.True(_decider.ShouldUpdateSnapshot());
        }

        [Theory]
        [InlineData("false")]
        [InlineData("False")]
        [InlineData("FALSE")]
        [InlineData("fAlSe")]
        public void FalseEnvironmentVariableSet_ShouldNotUpdate(string value)
        {
            Environment.SetEnvironmentVariable(_envVar, value);
            Assert.False(_decider.ShouldUpdateSnapshot());
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("randomstring")]
        [InlineData("")]
        public void InvalidEnvironmentVariableSet_ShouldNotUpdate(string value)
        {
            Environment.SetEnvironmentVariable(_envVar, value);
            Assert.False(_decider.ShouldUpdateSnapshot());
        }
    }
}
