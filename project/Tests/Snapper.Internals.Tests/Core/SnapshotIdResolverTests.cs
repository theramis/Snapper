using System.IO;
using FluentAssertions;
using Moq;
using Snapper.Attributes;
using Snapper.Core;
using Snapper.Core.TestMethodResolver;
using Xunit;

namespace Snapper.Internals.Tests.Core
{
    public class SnapshotIdResolverTests
    {
        private readonly SnapshotIdResolver _snapshotIdResolver;

        public SnapshotIdResolverTests()
        {
            var testMethodResolver = new TestMethodResolver();
            _snapshotIdResolver = new SnapshotIdResolver(testMethodResolver);
        }

        [Fact]
        public void ResolveSnapshotId_WithDefaultSettings()
        {
            var snapshotId = _snapshotIdResolver.ResolveSnapshotId(null, SnapshotSettings.New());

            var filePath = Path.Combine("Snapper.Internals.Tests", "Core",
                "_snapshots", $"{nameof(SnapshotIdResolverTests)}_{nameof(ResolveSnapshotId_WithDefaultSettings)}.json");

            snapshotId.FilePath.Should().EndWith(filePath);
            snapshotId.PrimaryId.Should().BeNull();
            snapshotId.SecondaryId.Should().BeNull();
        }

        [Fact]
        public void ResolveSnapshotId_WithChildSnapshotName_AndDefaultSettings()
        {
            var snapshotId = _snapshotIdResolver.ResolveSnapshotId("childSnapshotName", SnapshotSettings.New());

            var filePath = Path.Combine("Snapper.Internals.Tests", "Core",
                "_snapshots", $"{nameof(SnapshotIdResolverTests)}_{nameof(ResolveSnapshotId_WithChildSnapshotName_AndDefaultSettings)}.json");

            snapshotId.FilePath.Should().EndWith(filePath);
            snapshotId.PrimaryId.Should().Be("childSnapshotName");
            snapshotId.SecondaryId.Should().BeNull();
        }

        [Fact]
        public void ResolveSnapshotId_UsingSettingsOnly()
        {
            var settings = SnapshotSettings.New().SnapshotDirectory("dir")
                .SnapshotFileName("filename")
                .StoreSnapshotsPerClass(false);

            var mockTestResolver = new Mock<ITestMethodResolver>(MockBehavior.Strict);
            var resolver = new SnapshotIdResolver(mockTestResolver.Object);
            var snapshotId = resolver.ResolveSnapshotId(null, settings);

            var filePath = Path.Combine("dir", "filename.json");

            snapshotId.FilePath.Should().Be(filePath);
            snapshotId.PrimaryId.Should().BeNull();
            snapshotId.SecondaryId.Should().BeNull();
        }

        [Fact]
        public void ResolveSnapshotId_WithChildSnapshotName_UsingSettingsOnly()
        {
            var settings = SnapshotSettings.New().SnapshotDirectory("dir")
                .SnapshotFileName("filename")
                .StoreSnapshotsPerClass(false);

            var mockTestResolver = new Mock<ITestMethodResolver>(MockBehavior.Strict);
            var resolver = new SnapshotIdResolver(mockTestResolver.Object);
            var snapshotId = resolver.ResolveSnapshotId("childSnapshotName", settings);

            var filePath = Path.Combine("dir", "filename.json");

            snapshotId.FilePath.Should().Be(filePath);
            snapshotId.PrimaryId.Should().Be("childSnapshotName");
            snapshotId.SecondaryId.Should().BeNull();
        }

        [Fact]
        public void ResolveSnapshotId_UsingSettings_ForDirAndClassName()
        {
            var settings = SnapshotSettings.New().SnapshotDirectory("dir")
                .SnapshotClassName("className")
                .StoreSnapshotsPerClass(false);

            var snapshotId = _snapshotIdResolver.ResolveSnapshotId(null, settings);

            var filePath = Path.Combine("dir",
                $"className_{nameof(ResolveSnapshotId_UsingSettings_ForDirAndClassName)}.json");

            snapshotId.FilePath.Should().Be(filePath);
            snapshotId.PrimaryId.Should().BeNull();
            snapshotId.SecondaryId.Should().BeNull();
        }
    }

    [StoreSnapshotsPerClass]
    public class SnapshotIdResolverPerClassTests
    {
        private readonly SnapshotIdResolver _snapshotIdResolver;

        public SnapshotIdResolverPerClassTests()
        {
            var testMethodResolver = new TestMethodResolver();
            _snapshotIdResolver = new SnapshotIdResolver(testMethodResolver);
        }

        [Fact]
        public void ResolveSnapshotId_WithDefaultSettings()
        {
            var snapshotId = _snapshotIdResolver.ResolveSnapshotId(null, SnapshotSettings.New());

            var filePath = Path.Combine("Snapper.Internals.Tests", "Core",
                "_snapshots", $"{nameof(SnapshotIdResolverPerClassTests)}.json");

            snapshotId.FilePath.Should().EndWith(filePath);
            snapshotId.PrimaryId.Should().Be(nameof(ResolveSnapshotId_WithDefaultSettings));
            snapshotId.SecondaryId.Should().BeNull();
        }

        [Fact]
        public void ResolveSnapshotId_WithChildSnapshotName_AndDefaultSettings()
        {
            var snapshotId = _snapshotIdResolver.ResolveSnapshotId("childSnapshotName", SnapshotSettings.New());

            var filePath = Path.Combine("Snapper.Internals.Tests", "Core",
                "_snapshots", $"{nameof(SnapshotIdResolverPerClassTests)}.json");

            snapshotId.FilePath.Should().EndWith(filePath);
            snapshotId.PrimaryId.Should().Be(nameof(ResolveSnapshotId_WithChildSnapshotName_AndDefaultSettings));
            snapshotId.SecondaryId.Should().Be("childSnapshotName");
        }

        [Fact]
        public void ResolveSnapshotId_UsingSettingsOnly()
        {
            var settings = SnapshotSettings.New().SnapshotDirectory("dir")
                .SnapshotFileName("filename")
                .SnapshotTestName("testName")
                .StoreSnapshotsPerClass(true);

            var mockTestResolver = new Mock<ITestMethodResolver>(MockBehavior.Strict);
            var resolver = new SnapshotIdResolver(mockTestResolver.Object);
            var snapshotId = resolver.ResolveSnapshotId(null, settings);

            var filePath = Path.Combine("dir", "filename.json");

            snapshotId.FilePath.Should().Be(filePath);
            snapshotId.PrimaryId.Should().Be("testName");
            snapshotId.SecondaryId.Should().BeNull();
        }

        [Fact]
        public void ResolveSnapshotId_WithChildSnapshotName_UsingSettingsOnly()
        {
            var settings = SnapshotSettings.New().SnapshotDirectory("dir")
                .SnapshotFileName("filename")
                .SnapshotTestName("testName")
                .StoreSnapshotsPerClass(true);

            var mockTestResolver = new Mock<ITestMethodResolver>(MockBehavior.Strict);
            var resolver = new SnapshotIdResolver(mockTestResolver.Object);
            var snapshotId = resolver.ResolveSnapshotId("childSnapshotName", settings);

            var filePath = Path.Combine("dir", "filename.json");

            snapshotId.FilePath.Should().Be(filePath);
            snapshotId.PrimaryId.Should().Be("testName");
            snapshotId.SecondaryId.Should().Be("childSnapshotName");
        }

        [Fact]
        public void ResolveSnapshotId_UsingSettings_ForDirAndClassName()
        {
            var settings = SnapshotSettings.New().SnapshotDirectory("dir")
                .SnapshotClassName("className");

            var snapshotId = _snapshotIdResolver.ResolveSnapshotId(null, settings);

            var filePath = Path.Combine("dir", "className.json");

            snapshotId.FilePath.Should().Be(filePath);
            snapshotId.PrimaryId.Should().Be(nameof(ResolveSnapshotId_UsingSettings_ForDirAndClassName));
            snapshotId.SecondaryId.Should().BeNull();
        }

        [Fact]
        public void ResolveSnapshotId_StoreSnapshotsPerClassInSettings_OverridesAttribute()
        {
            var settings = SnapshotSettings.New().SnapshotDirectory("dir")
                .SnapshotClassName("className")
                .SnapshotTestName("testName")
                .StoreSnapshotsPerClass(false);

            var mockTestResolver = new Mock<ITestMethodResolver>(MockBehavior.Strict);
            var resolver = new SnapshotIdResolver(mockTestResolver.Object);
            var snapshotId = resolver.ResolveSnapshotId(null, settings);

            var filePath = Path.Combine("dir", "className_testName.json");

            snapshotId.FilePath.Should().Be(filePath);
            snapshotId.PrimaryId.Should().BeNull();
            snapshotId.SecondaryId.Should().BeNull();
        }
    }
}
