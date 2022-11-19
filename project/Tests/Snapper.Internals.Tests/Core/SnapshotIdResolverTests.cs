using System.IO;
using FluentAssertions;
using Snapper.Attributes;
using Snapper.Core;
using Snapper.Core.TestMethodResolver;
using Xunit;

namespace Snapper.Internals.Tests.Core
{
    // TODO add more tests here
    public class SnapshotIdResolverTests
    {
        private readonly SnapshotIdResolver _snapshotIdResolver;

        public SnapshotIdResolverTests()
        {
            var testMethodResolver = new TestMethodResolver();
            _snapshotIdResolver = new SnapshotIdResolver(testMethodResolver);
        }

        [Fact]
        public void ResolveSnapshotId()
        {
            var snapshotId = _snapshotIdResolver.ResolveSnapshotId(null, SnapshotSettings.New());

            var filePath = Path.Combine("Snapper.Internals.Tests", "Core",
                "_snapshots", $"{nameof(SnapshotIdResolverTests)}_{nameof(ResolveSnapshotId)}.json");

            snapshotId.FilePath.Should().EndWith(filePath);
            snapshotId.PrimaryId.Should().BeNull();
            snapshotId.SecondaryId.Should().BeNull();
        }

        [Fact]
        public void ResolveSnapshotId_WithPartialName()
        {
            var snapshotId = _snapshotIdResolver.ResolveSnapshotId("partialName", SnapshotSettings.New());

            var filePath = Path.Combine("Snapper.Internals.Tests", "Core",
                "_snapshots", $"{nameof(SnapshotIdResolverTests)}_{nameof(ResolveSnapshotId_WithPartialName)}.json");

            snapshotId.FilePath.Should().EndWith(filePath);
            snapshotId.PrimaryId.Should().Be("partialName");
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
        public void ResolveSnapshotId()
        {
            var snapshotId = _snapshotIdResolver.ResolveSnapshotId(null, SnapshotSettings.New());

            var filePath = Path.Combine("Snapper.Internals.Tests", "Core",
                "_snapshots", $"{nameof(SnapshotIdResolverPerClassTests)}.json");

            snapshotId.FilePath.Should().EndWith(filePath);
            snapshotId.PrimaryId.Should().Be(nameof(ResolveSnapshotId));
            snapshotId.SecondaryId.Should().BeNull();
        }

        [Fact]
        public void ResolveSnapshotId_WithPartialName()
        {
            var snapshotId = _snapshotIdResolver.ResolveSnapshotId("partialName", SnapshotSettings.New());

            var filePath = Path.Combine("Snapper.Internals.Tests", "Core",
                "_snapshots", $"{nameof(SnapshotIdResolverPerClassTests)}.json");

            snapshotId.FilePath.Should().EndWith(filePath);
            snapshotId.PrimaryId.Should().Be(nameof(ResolveSnapshotId_WithPartialName));
            snapshotId.SecondaryId.Should().Be("partialName");
        }
    }
}
