using System;
using FluentAssertions;
using Moq;
using Snapper.Core;
using Xunit;

namespace Snapper.Internals.Tests.Core
{
    internal class SnapperCoreProxy : SnapperCore
    {
        public SnapperCoreProxy(ISnapshotStore snapshotStore,
            ISnapshotUpdateDecider snapshotUpdateDecider,
            ISnapshotComparer snapshotComparer)
            : base(snapshotStore, snapshotUpdateDecider, snapshotComparer)
        {
        }

        public new SnapResult Snap(SnapshotId snapshotId, object newSnapshot)
        {
            return base.Snap(snapshotId, newSnapshot);
        }
    }

    public class SnapperCoreTests
    {
        private readonly object _obj = new {value = 1};
        private readonly SnapperCoreProxy _snapper;
        private readonly Mock<ISnapshotStore> _store;
        private readonly Mock<ISnapshotUpdateDecider> _updateDecider;
        private readonly Mock<ISnapshotComparer> _comparer;

        public SnapperCoreTests()
        {
            _store = new Mock<ISnapshotStore>();
            _updateDecider = new Mock<ISnapshotUpdateDecider>();
            _comparer = new Mock<ISnapshotComparer>();

            _snapper = new SnapperCoreProxy(_store.Object, _updateDecider.Object,
                _comparer.Object);
        }

        [Fact]
        public void SnapshotMatches_ResultStatusIs_SnapshotsMatch()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);
            _comparer.Setup(a => a.CompareSnapshots(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(true);

            var result = _snapper.Snap(new SnapshotId("name", null, null, null), _obj);

            result.Status.Should().Be(SnapResultStatus.SnapshotsMatch);
            result.OldSnapshot.Should().BeEquivalentTo(_obj);
            result.NewSnapshot.Should().BeEquivalentTo(_obj);
        }

        [Fact]
        public void SnapshotMatches_ShouldUpdate_ResultStatusIs_SnapshotsMatch()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(true);
            _comparer.Setup(a => a.CompareSnapshots(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(true);

            var result = _snapper.Snap(new SnapshotId("name", null, null, null), _obj);

            _store.Verify(a => a.StoreSnapshot(It.IsAny<SnapshotId>(), It.IsAny<object>()), Times.Never);
            result.Status.Should().Be(SnapResultStatus.SnapshotsMatch);
            result.OldSnapshot.Should().BeEquivalentTo(_obj);
            result.NewSnapshot.Should().BeEquivalentTo(_obj);
        }

        [Fact]
        public void SnapshotDoesNotMatch_ResultStatusIs_SnapshotsDoNotMatch()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);
            _comparer.Setup(a => a.CompareSnapshots(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(false);

            var newObj = new {value = 2};
            var result = _snapper.Snap(new SnapshotId("name", null, null, null), newObj);

            result.Status.Should().Be(SnapResultStatus.SnapshotsDoNotMatch);
            result.OldSnapshot.Should().BeEquivalentTo(_obj);
            result.NewSnapshot.Should().BeEquivalentTo(newObj);
        }

        [Fact]
        public void SnapshotDoesNotMatch_ShouldUpdate_ResultStatusIs_SnapshotUpdated()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(true);
            _comparer.Setup(a => a.CompareSnapshots(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(false);

            var newObj = new {value = 2};
            var result = _snapper.Snap(new SnapshotId("name", null, null, null), newObj);

            _store.Verify(a => a.StoreSnapshot(It.IsAny<SnapshotId>(), It.IsAny<object>()), Times.Once);
            result.Status.Should().Be(SnapResultStatus.SnapshotUpdated);
            result.OldSnapshot.Should().BeEquivalentTo(_obj);
            result.NewSnapshot.Should().BeEquivalentTo(newObj);
        }

        [Fact]
        public void SnapshotDoesNotExist_ShouldUpdate_ResultStatusIs_SnapshotUpdated()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(null);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(true);

            var result = _snapper.Snap(new SnapshotId("name", null, null, null), _obj);

            _store.Verify(a => a.StoreSnapshot(It.IsAny<SnapshotId>(), It.IsAny<object>()), Times.Once);
            result.Status.Should().Be(SnapResultStatus.SnapshotUpdated);
            result.OldSnapshot.Should().BeNull();
            result.NewSnapshot.Should().BeEquivalentTo(_obj);
        }

        [Fact]
        public void SnapshotDoesNotExist_ResultStatusIs_SnapshotUpdated()
        {
            // Tests run on CI so clearing the CI environment variable to emulate local machine
            Environment.SetEnvironmentVariable("CI", null, EnvironmentVariableTarget.Process);

            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(null);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);

            var result = _snapper.Snap(new SnapshotId("name", null, null, null), _obj);

            _store.Verify(a => a.StoreSnapshot(It.IsAny<SnapshotId>(), It.IsAny<object>()), Times.Once);
            result.Status.Should().Be(SnapResultStatus.SnapshotUpdated);
            result.OldSnapshot.Should().BeNull();
            result.NewSnapshot.Should().BeEquivalentTo(_obj);
        }

        [Fact]
        public void SnapshotDoesNotExist_And_IsCiEnv_ResultStatusIs_SnapshotDoesNotExist()
        {
            Environment.SetEnvironmentVariable("CI", "true", EnvironmentVariableTarget.Process);
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(null);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);

            var result = _snapper.Snap(new SnapshotId("name", null, null, null), _obj);

            result.Status.Should().Be(SnapResultStatus.SnapshotDoesNotExist);
            result.OldSnapshot.Should().BeNull();
            result.NewSnapshot.Should().BeEquivalentTo(_obj);
            Environment.SetEnvironmentVariable("CI", null, EnvironmentVariableTarget.Process);
        }
    }
}
