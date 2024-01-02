using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Snapper.Attributes;
using Xunit;

namespace Snapper.Tests
{
    public class SnapperSnapshotsPerMethodTests : BaseTest
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
        [UpdateSnapshots(false)]
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
        public void SnapshotDoesNotExist_And_IsCiEnv_SnapshotDoesNotExistException_IsThrown()
        {
            // Arrange
            Environment.SetEnvironmentVariable("CI", "true", EnvironmentVariableTarget.Process);
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
                          $"Run the test outside of a CI environment to create a snapshot.{Environment.NewLine}", exception.Message);

            // Cleanup
            Environment.SetEnvironmentVariable("CI", null, EnvironmentVariableTarget.Process);
        }

        [Fact]
        public void SnapshotsDoesNotExist_SnapshotIsCreated()
        {
            // Tests run on CI so clearing the CI environment variable to emulate local machine
            Environment.SetEnvironmentVariable("CI", null, EnvironmentVariableTarget.Process);

            // Arrange
            var snapshotFilePath = GetSnapshotFilePath<SnapperSnapshotsPerMethodTests>(
                nameof(SnapshotsDoesNotExist_SnapshotIsCreated));

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

        [Fact]
        [UpdateSnapshots(false)]
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

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void PrimitiveIntSnapshot()
        {
            1.ShouldMatchSnapshot();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2.1)]
        [InlineData(true)]
        [InlineData('a')]
        [InlineData("string")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void PrimitivesTheorySnapshots(object obj)
        {
            obj.ShouldMatchChildSnapshot(obj.ToString() ?? string.Empty);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ArraySnapshot()
        {
            new List<int> { 1, 2, 3 }.ShouldMatchSnapshot();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2.1)]
        [InlineData(true)]
        [InlineData('a')]
        [InlineData("string")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ArrayTheorySnapshots(object obj)
        {
            var list = new List<object> { obj, obj, obj };
            list.ShouldMatchChildSnapshot(obj.ToString() ?? string.Empty);
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
