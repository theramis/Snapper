using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
            Assert.That(snapshot, Matches.Snapshot(SnapshotSettings.New()
                .SnapshotDirectory(Path.Combine(GetCurrentClassDirectory(), "_custom"))));
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
            Assert.That(snapshot, Matches.ChildSnapshot($"{data}", SnapshotSettings.New()
                .SnapshotDirectory(Path.Combine(GetCurrentClassDirectory(), "_custom"))));
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
            Assert.That(snapshot, Matches.ChildSnapshot($"{data}", SnapshotSettings.New()
                .SnapshotDirectory(Path.Combine(GetCurrentClassDirectory(), "_custom"))
                .StoreSnapshotsPerClass(true)));
        }

        private static string GetCurrentClassDirectory([CallerFilePath] string callerFilePath = "")
            => Path.GetDirectoryName(callerFilePath);
    }
}
