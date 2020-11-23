using FluentAssertions;
using Moq;
using Snapper.Core;
using Xunit;

namespace Snapper.Internals.Tests.Core
{
    public class CoreSnapperHandlerTests
    {
        private readonly object _obj = new { value = 1 };
        private readonly CoreSnapshotHandler _snapper;
        private readonly Mock<ISnapshotStore> _store;
        private readonly Mock<ISnapshotUpdateDecider> _updateDecider;
        private readonly Mock<ISnapshotComparer> _comparer;

        public CoreSnapperHandlerTests()
        {
            _store = new Mock<ISnapshotStore>();
            _updateDecider = new Mock<ISnapshotUpdateDecider>();
            _comparer = new Mock<ISnapshotComparer>();

            _snapper = new CoreSnapshotHandler(
                _store.Object,
                _comparer.Object,
                _updateDecider.Object);
        }

        [Fact]
        public void SnapshotMatches_ResultStatusIs_SnapshotsMatch()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);
            _comparer.Setup(a => a.CompareSnapshots(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(SnapResult.SnapshotsMatch(_obj, _obj));

            var result = _snapper.Snap(new SnapshotId("name", null, null, null), _obj);

            result.Should().BeEquivalentTo(SnapResult.SnapshotsMatch(_obj, _obj));
        }

        [Fact]
        public void SnapshotMatches_ShouldUpdate_ResultStatusIs_SnapshotsMatch()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(true);
            _comparer.Setup(a => a.CompareSnapshots(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(SnapResult.SnapshotsMatch(_obj, _obj));

            var result = _snapper.Snap(new SnapshotId("name", null, null, null), _obj);

            _store.Verify(a => a.StoreSnapshot(It.IsAny<SnapshotId>(), It.IsAny<object>()), Times.Never);
            result.Should().BeEquivalentTo(SnapResult.SnapshotsMatch(_obj, _obj));
        }

        [Fact]
        public void SnapshotDoesNotMatch_ResultStatusIs_SnapshotsDoNotMatch()
        {
            var newObj = new { value = 2 };

            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);
            _comparer.Setup(a => a.CompareSnapshots(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(SnapResult.SnapshotsDoNotMatch(_obj, newObj));

            var result = _snapper.Snap(new SnapshotId("name", null, null, null), newObj);

            result.Should().BeEquivalentTo(SnapResult.SnapshotsDoNotMatch(_obj, newObj));
        }

        [Fact]
        public void SnapshotDoesNotMatch_ShouldUpdate_ResultStatusIs_SnapshotUpdated()
        {
            var newObj = new { value = 2 };

            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(true);
            _comparer.Setup(a => a.CompareSnapshots(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(SnapResult.SnapshotsDoNotMatch(_obj, newObj));

            var result = _snapper.Snap(new SnapshotId("name", null, null, null), newObj);

            _store.Verify(a => a.StoreSnapshot(It.IsAny<SnapshotId>(), It.IsAny<object>()), Times.Once);
            result.Should().BeEquivalentTo(SnapResult.SnapshotUpdated(_obj, newObj));
        }

        [Fact]
        public void SnapshotDoesNotExist_ShouldUpdate_ResultStatusIs_SnapshotUpdated()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(null);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(true);
            _comparer.Setup(a => a.CompareSnapshots(It.IsAny<object>(), It.IsAny<object>()))
               .Returns(SnapResult.SnapshotDoesNotExist(_obj));

            var result = _snapper.Snap(new SnapshotId("name", null, null, null), _obj);

            _store.Verify(a => a.StoreSnapshot(It.IsAny<SnapshotId>(), It.IsAny<object>()), Times.Once);
            result.Should().BeEquivalentTo(SnapResult.SnapshotUpdated(null, _obj));
        }

        [Fact]
        public void SnapshotDoesNotExist_ResultStatusIs_SnapshotDoesNotExist()
        {
            _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(null);
            _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);
            _comparer.Setup(a => a.CompareSnapshots(It.IsAny<object>(), It.IsAny<object>()))
               .Returns(SnapResult.SnapshotDoesNotExist(_obj));

            var result = _snapper.Snap(new SnapshotId("name", null, null, null), _obj);

            result.Should().BeEquivalentTo(SnapResult.SnapshotDoesNotExist(_obj));
        }
    }
}
