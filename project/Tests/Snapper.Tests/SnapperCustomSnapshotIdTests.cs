using System.IO;
using System.Runtime.CompilerServices;
using Snapper.Core;
using Xunit;

namespace Snapper.Tests
{
    public class SnapperCustomSnapshotIdTests
    {
        [Fact]
        public void SnapshotsMatch()
        {
            // Arrange
            var snapshot = new
            {
                TestValue = "value"
            };

            // Act/Assert
            snapshot.ShouldMatchSnapshot(new SnapshotId(
                Path.Combine(GetCurrentClassDirectory(), "_custom"),
                nameof(SnapperCustomSnapshotIdTests),
                nameof(SnapshotsMatch)));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void TheorySnapshotsMatch(int data)
        {
            // Arrange
            var snapshot = new
            {
                TestValue = data
            };

            // Act/Assert
            snapshot.ShouldMatchSnapshot(new SnapshotId(
                Path.Combine(GetCurrentClassDirectory(), "_custom"),
                nameof(SnapperCustomSnapshotIdTests),
                nameof(TheorySnapshotsMatch),
                data.ToString()));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void TheorySnapshotsMatchWithPerClass(int data)
        {
            // Arrange
            var snapshot = new
            {
                TestValue = data
            };

            // Act/Assert
            snapshot.ShouldMatchSnapshot(new SnapshotId(
                Path.Combine(GetCurrentClassDirectory(), "_custom"),
                nameof(SnapperCustomSnapshotIdTests),
                nameof(TheorySnapshotsMatchWithPerClass),
                data.ToString(),
                true));
        }

        private static string GetCurrentClassDirectory([CallerFilePath] string callerFilePath = "")
            => Path.GetDirectoryName(callerFilePath);
    }
}
