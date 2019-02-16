using FluentAssertions;
using Moq;
using Snapper.Core;
using Xunit;

namespace Snapper.Internals.Tests.Core
{
    public class SnapperCoreTests
    {
        private readonly object _obj = new {value = 1};
        private readonly SnapperCore _snapper;
        private readonly Mock<ISnapshotStore> _store;
        private readonly Mock<ISnapshotUpdateDecider> _updateDecider;
        private readonly Mock<ISnapshotComparer> _comparer;

        public SnapperCoreTests()
        {
            _store = new Mock<ISnapshotStore>();
            _updateDecider = new Mock<ISnapshotUpdateDecider>();
            _comparer = new Mock<ISnapshotComparer>();

            _snapper = new SnapperCore(_store.Object, _updateDecider.Object,
                _comparer.Object);
        }

        [Fact]
        public void SnapshotMatches_ResultStatusIs_SnapshotsMatch()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);
            _comparer.Setup(a => a.CompareSnapshots(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(true);

            var result = _snapper.Snap(new SnapshotId("name"), _obj);

            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotsMatch);
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

            var result = _snapper.Snap(new SnapshotId("name"), _obj);

            _store.Verify(a => a.StoreSnapshot(It.IsAny<SnapshotId>(), It.IsAny<object>()), Times.Never);
            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotsMatch);
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
            var result = _snapper.Snap(new SnapshotId("name"), newObj);

            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotsDoNotMatch);
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
            var result = _snapper.Snap(new SnapshotId("name"), newObj);

            _store.Verify(a => a.StoreSnapshot(It.IsAny<SnapshotId>(), It.IsAny<object>()), Times.Once);
            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotUpdated);
            result.OldSnapshot.Should().BeEquivalentTo(_obj);
            result.NewSnapshot.Should().BeEquivalentTo(newObj);
        }

        [Fact]
        public void SnapshotDoesNotExist_ShouldUpdate_ResultStatusIs_SnapshotUpdated()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(null);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(true);

            var result = _snapper.Snap(new SnapshotId("name"), _obj);

            _store.Verify(a => a.StoreSnapshot(It.IsAny<SnapshotId>(), It.IsAny<object>()), Times.Once);
            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotUpdated);
            result.OldSnapshot.Should().BeNull();
            result.NewSnapshot.Should().BeEquivalentTo(_obj);
        }

        [Fact]
        public void SnapshotDoesNotExist_ResultStatusIs_SnapshotDoesNotExist()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(null);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);

            var result = _snapper.Snap(new SnapshotId("name"), _obj);

            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotDoesNotExist);
            result.OldSnapshot.Should().BeNull();
            result.NewSnapshot.Should().BeEquivalentTo(_obj);
        }
    }
}
