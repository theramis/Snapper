using System;
using System.IO;
using System.Runtime.CompilerServices;
using Snapper.Attributes;
using Xunit;

namespace Snapper.Tests
{
    [StoreSnapshotsPerClass]
    public class SnapperSnapshotsPerClassTests
    {
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void SnapshotsMatch()
        {
            // Arrange
            var snapshot = new
            {
                TestValue = "value"
            };

            // Act/Assert
            snapshot.ShouldMatchSnapshot();
        }

        [Fact]
        public void SnapshotsDoNotMatch_SnapshotsDoNotMatchException_IsThrown()
        {
            // Arrange
            var snapshot = new
            {
                TestValue = "value",
                NewValue = "newvalue"
            };

            // Act
            var exception = Record.Exception(() => snapshot.ShouldMatchSnapshot());

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Snapper.Exceptions.SnapshotsDoNotMatchException", exception.GetType().FullName);
            Assert.Equal(string.Join(
                Environment.NewLine,
                "",
                "Snapshots do not match",
                "- Snapshot",
                "+ Received",
                "",
                "",
                "{",
                "-    \"TestValue\": \"value\"",
                "+    \"TestValue\": \"value\",",
                "+    \"NewValue\": \"newvalue\"",
                "}" + Environment.NewLine
                ), exception.Message);
        }

        [Fact]
        [UpdateSnapshots(false)]
        public void SnapshotsDoNotMatch_UpdateSnapshotsAttributeIsSet_SnapshotIsUpdated()
        {
            // Arrange
            var snapshotFilePath = GetSnapshotFilePath<SnapperSnapshotsPerClassTests>();

            var content = File.ReadAllText(snapshotFilePath);
            var newContent = content.Replace("isUpdated", "doesNotMatch");
            File.WriteAllText(snapshotFilePath, newContent);

            var snapshot = new
            {
                TestValue = "isUpdated"
            };

            // Act
            snapshot.ShouldMatchSnapshot();

            // Assert
            var updatedSnapshotContent = File.ReadAllText(snapshotFilePath);
            Assert.Contains("\"TestValue\": \"isUpdated\"", updatedSnapshotContent);
        }

        [Fact]
        public void SnapshotDoesNotExist_SnapshotDoesNotExistException_IsThrown()
        {
            // Arrange
            var snapshot = new
            {
                TestValue = "value"
            };

            // Act
            var exception = Record.Exception(() => snapshot.ShouldMatchSnapshot());

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Snapper.Exceptions.SnapshotDoesNotExistException", exception.GetType().FullName);
            Assert.Equal( $"A snapshot does not exist.{Environment.NewLine}{Environment.NewLine}" +
                          "Apply the [UpdateSnapshots] attribute on the " +
                          "test method or class and then run the test again to " +
                          $"create a snapshot.{Environment.NewLine}", exception.Message);
        }

        [Fact]
        [UpdateSnapshots(false)]
        public void SnapshotsDoesNotExist_UpdateSnapshotsAttributeIsSet_SnapshotIsCreated()
        {
            // Arrange
            var snapshotFilePath = GetSnapshotFilePath<SnapperSnapshotsPerClassTests>();

            var content = File.ReadAllText(snapshotFilePath);
            var snapshotToRemove = string.Join(
                Environment.NewLine,
                "  \"SnapshotsDoesNotExist_UpdateSnapshotsAttributeIsSet_SnapshotIsCreated\": {",
                "    \"TestValue\": \"doesNotExist\"",
                "  }");

            var newContent = content.Replace(snapshotToRemove, "");
            File.WriteAllText(snapshotFilePath, newContent);

            var snapshot = new
            {
                TestValue = "doesNotExist"
            };

            // Act
            snapshot.ShouldMatchSnapshot();

            // Assert
            Assert.Contains(snapshotToRemove, File.ReadAllText(snapshotFilePath));

            // Cleanup
            File.WriteAllText(snapshotFilePath, newContent);
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
            snapshot.ShouldMatchChildSnapshot(data.ToString());
        }

        private static string GetSnapshotFilePath<T>([CallerFilePath] string callerFilePath = "")
        {
            var testFileDirectory = Path.GetDirectoryName(callerFilePath);
            var classType = typeof(T);

            return Path.Combine(testFileDirectory, "_snapshots",
                $"{classType.Name}.json");
        }
    }
}
