using System.IO;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Snapper.Core;

namespace Snapper.Nunit.Tests
{
    public class NunitSnapperCustomTests
    {
        [Test]
        public void SnapshotsMatch()
        {
            // Arrange
            var snapshot = new
            {
                TestValue = "value"
            };

            // Act/Assert
            Assert.That(snapshot, Matches.Snapshot(new SnapshotId(
                Path.Combine(GetCurrentClassDirectory(), "_custom"),
                nameof(NunitSnapperCustomTests),
                nameof(SnapshotsMatch))));
        }

        [DatapointSource]
        public int[] values = new int[] { 1, 2, 3 };

        [Theory]
        public void TheorySnapshotsMatch(int data)
        {
            // Arrange
            var snapshot = new
            {
                TestValue = data
            };

            // Act/Assert
            Assert.That(snapshot, Matches.Snapshot(new SnapshotId(
                Path.Combine(GetCurrentClassDirectory(), "_custom"),
                nameof(NunitSnapperCustomTests),
                nameof(TheorySnapshotsMatch),
                data.ToString())));
        }

        [Theory]
        public void TheorySnapshotsMatchWithPerClass(int data)
        {
            // Arrange
            var snapshot = new
            {
                TestValue = data
            };

            // Act/Assert
            Assert.That(snapshot, Matches.Snapshot(new SnapshotId(
                Path.Combine(GetCurrentClassDirectory(), "_custom"),
                nameof(NunitSnapperCustomTests),
                nameof(TheorySnapshotsMatchWithPerClass),
                data.ToString(),
                true)));
        }

        private static string GetCurrentClassDirectory([CallerFilePath] string callerFilePath = "")
            => Path.GetDirectoryName(callerFilePath);
    }
}
