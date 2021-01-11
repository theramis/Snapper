using System;
using FluentAssertions;
using Snapper.Attributes;
using Snapper.Core;
using Snapper.Core.TestMethodResolver;
using Xunit;

namespace Snapper.Internals.Tests.Core
{
    public class SnapshotUpdateDeciderTests
    {
        private readonly string _envVar;
        private readonly SnapshotUpdateDecider _decider;

        public SnapshotUpdateDeciderTests()
        {
            _envVar = Guid.NewGuid().ToString();
            _decider = new SnapshotUpdateDecider(new TestMethodResolver(), _envVar);
        }

        [Fact]
        public void NoEnvironmentVariable_ShouldNotUpdate()
        {
            _decider.ShouldUpdateSnapshot().Should().BeFalse();
        }

        [Theory]
        [InlineData("true")]
        [InlineData("True")]
        [InlineData("TRUE")]
        [InlineData("tRuE")]
        public void TrueEnvironmentVariableSet_ShouldUpdate(string value)
        {
            Environment.SetEnvironmentVariable(_envVar, value, EnvironmentVariableTarget.Process);
            _decider.ShouldUpdateSnapshot().Should().BeTrue();
            Environment.SetEnvironmentVariable(_envVar, null, EnvironmentVariableTarget.Process);
        }

        [Theory]
        [InlineData("false")]
        [InlineData("False")]
        [InlineData("FALSE")]
        [InlineData("fAlSe")]
        public void FalseEnvironmentVariableSet_ShouldNotUpdate(string value)
        {
            Environment.SetEnvironmentVariable(_envVar, value, EnvironmentVariableTarget.Process);
            _decider.ShouldUpdateSnapshot().Should().BeFalse();
            Environment.SetEnvironmentVariable(_envVar, null, EnvironmentVariableTarget.Process);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("randomstring")]
        [InlineData("")]
        public void InvalidEnvironmentVariableSet_ShouldNotUpdate(string value)
        {
            Environment.SetEnvironmentVariable(_envVar, value, EnvironmentVariableTarget.Process);
            _decider.ShouldUpdateSnapshot().Should().BeFalse();
            Environment.SetEnvironmentVariable(_envVar, null, EnvironmentVariableTarget.Process);
        }

        [Fact]
        [UpdateSnapshots(false)]
        public void UpdateAttribute_NoEnvironmentVariable_ShouldUpdate()
        {
            _decider.ShouldUpdateSnapshot().Should().BeTrue();
        }

        [Fact]
        [UpdateSnapshots(false)]
        public void UpdateAttribute_TrueEnvironmentVariableSet_ShouldUpdate()
        {
            Environment.SetEnvironmentVariable(_envVar, "true", EnvironmentVariableTarget.Process);
            _decider.ShouldUpdateSnapshot().Should().BeTrue();
            Environment.SetEnvironmentVariable(_envVar, null, EnvironmentVariableTarget.Process);
        }

        [Fact]
        [UpdateSnapshots(false)]
        public void UpdateAttribute_FalseEnvironmentVariableSet_ShouldUpdate()
        {
            Environment.SetEnvironmentVariable(_envVar, "False", EnvironmentVariableTarget.Process);
            _decider.ShouldUpdateSnapshot().Should().BeTrue();
            Environment.SetEnvironmentVariable(_envVar, null, EnvironmentVariableTarget.Process);
        }
    }

    public class SnapshotUpdateDeciderUpdateAttributeOnClassTests
    {
        private readonly SnapshotUpdateDecider _decider;

        public SnapshotUpdateDeciderUpdateAttributeOnClassTests()
        {
            _decider = new SnapshotUpdateDecider(new TestMethodResolver(), string.Empty);
        }

        [Fact]
        [UpdateSnapshots(false)]
        public void FirstMethod_ShouldUpdate()
        {
            _decider.ShouldUpdateSnapshot().Should().BeTrue();
        }

        [Fact]
        [UpdateSnapshots(false)]
        public void SecondMethod_ShouldUpdate()
        {
            _decider.ShouldUpdateSnapshot().Should().BeTrue();
        }
    }
}
