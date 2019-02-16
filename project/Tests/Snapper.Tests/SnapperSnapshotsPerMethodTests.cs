using System;
using System.IO;
using System.Runtime.CompilerServices;
using Snapper.Attributes;
using Xunit;

namespace Snapper.Tests
{
    public class SnapperSnapshotsPerMethodTests
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
        [UpdateSnapshots]
        public void SnapshotsMatch_UpdateSnapshotsAttributeIsSet_SnapshotIsUntouched()
        {
            // Arrange
            var snapshotFilePath = GetSnapshotFilePath<SnapperSnapshotsPerMethodTests>(
                nameof(SnapshotsMatch_UpdateSnapshotsAttributeIsSet_SnapshotIsUntouched));

            const string snapshotContent = "{       \"TestValue\": \"value\"       }";
            File.WriteAllText(snapshotFilePath, snapshotContent);

            var snapshot = new
            {
                TestValue = "value"
            };

            // Act
            snapshot.ShouldMatchSnapshot();

            // Assert
            Assert.Equal(snapshotContent, File.ReadAllText(snapshotFilePath));
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
        [UpdateSnapshots]
        public void SnapshotsDoNotMatch_UpdateSnapshotsAttributeIsSet_SnapshotIsUpdated()
        {
            // Arrange
            var snapshotFilePath = GetSnapshotFilePath<SnapperSnapshotsPerMethodTests>(
                    nameof(SnapshotsDoNotMatch_UpdateSnapshotsAttributeIsSet_SnapshotIsUpdated));
            var snapshotContent = string.Join(
                Environment.NewLine,
                "{",
                "  \"TestValue\": \"value\"",
                "}");
            File.WriteAllText(snapshotFilePath, snapshotContent);

            var snapshot = new
            {
                TestValue = "value",
                NewValue = "newvalue"
            };

            // Act
            snapshot.ShouldMatchSnapshot();

            // Assert
            var expectedSnapshotContent = string.Join(
                Environment.NewLine,
                "{",
                "  \"TestValue\": \"value\",",
                "  \"NewValue\": \"newvalue\"",
                "}");
            Assert.Equal(expectedSnapshotContent, File.ReadAllText(snapshotFilePath));
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
            Assert.Equal( $"A snapshot does not exist.{Environment.NewLine}{Environment.NewLine}", exception.Message);
        }

        [Fact]
        [UpdateSnapshots]
        public void SnapshotsDoesNotExist_UpdateSnapshotsAttributeIsSet_SnapshotIsCreated()
        {
            // Arrange
            var snapshotFilePath = GetSnapshotFilePath<SnapperSnapshotsPerMethodTests>(
                nameof(SnapshotsDoesNotExist_UpdateSnapshotsAttributeIsSet_SnapshotIsCreated));

            if (File.Exists(snapshotFilePath))
                File.Delete(snapshotFilePath);

            var snapshot = new
            {
                TestValue = "value"
            };

            // Act
            snapshot.ShouldMatchSnapshot();

            // Assert
            Assert.True(File.Exists(snapshotFilePath));

            var expectedSnapshotContent = string.Join(
                Environment.NewLine,
                "{",
                "  \"TestValue\": \"value\"",
                "}");
            Assert.Equal(expectedSnapshotContent, File.ReadAllText(snapshotFilePath));

            // Cleanup
            File.Delete(snapshotFilePath);
        }

        private static string GetSnapshotFilePath<T>(string methodName, [CallerFilePath] string callerFilePath = "")
        {
            var testFileDirectory = Path.GetDirectoryName(callerFilePath);
            var classType = typeof(T);

            return Path.Combine(testFileDirectory, "_snapshots",
                $"{classType.Name}_{methodName}.json");
        }
    }
}
