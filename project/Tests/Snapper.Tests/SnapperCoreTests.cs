using FluentAssertions;
using Moq;
using Snapper.Core;
using Xunit;

namespace Snapper.Tests
{
    public class SnapperCoreTests
    {
        private readonly object _obj = new {value = 1};
        private readonly SnapperCore _snapper;
        private readonly Mock<ISnapStore> _store;
        private readonly Mock<ISnapUpdateDecider> _updateDecider;
        private readonly Mock<ISnapComparer> _comparer;
        private readonly Mock<ISnapIdResolver> _idResolver;

        public SnapperCoreTests()
        {
            _store = new Mock<ISnapStore>();
            _updateDecider = new Mock<ISnapUpdateDecider>();
            _comparer = new Mock<ISnapComparer>();
            _idResolver = new Mock<ISnapIdResolver>();

            _snapper = new SnapperCore(_store.Object, _updateDecider.Object,
                _comparer.Object, _idResolver.Object);
        }

        [Fact]
        public void SnapshotDoesNotExist_ResultStatusIs_SnapshotDoesNotExist()
        {
            _idResolver.Setup(a => a.ResolveSnapId(It.IsAny<string>())).Returns("id");
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(null);
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(false);

            var result = _snapper.Snap("name", _obj);

            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotDoesNotExist);
            result.OldSnapshot.Should().BeNull();
            result.NewSnapshot.Should().BeEquivalentTo(_obj);
        }

        [Fact]
        public void SnapshotMatches_ResultStatusIs_SnapshotsMatch()
        {
            _idResolver.Setup(a => a.ResolveSnapId(It.IsAny<string>())).Returns("id");
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(false);
            _comparer.Setup(a => a.Compare(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(true);

            var result = _snapper.Snap("name", _obj);

            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotsMatch);
            result.OldSnapshot.Should().BeEquivalentTo(_obj);
            result.NewSnapshot.Should().BeEquivalentTo(_obj);
        }

        [Fact]
        public void SnapshotDoesNotMatch_ResultStatusIs_SnapshotsDoNotMatch()
        {
            _idResolver.Setup(a => a.ResolveSnapId(It.IsAny<string>())).Returns("id");
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(false);
            _comparer.Setup(a => a.Compare(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(false);

            var newObj = new {value = 2};
            var result = _snapper.Snap("name",  newObj);

            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotsDoNotMatch);
            result.OldSnapshot.Should().BeEquivalentTo(_obj);
            result.NewSnapshot.Should().BeEquivalentTo(newObj);
        }

        [Fact]
        public void SnapshotDoesNotMatch_ShouldUpdate_ResultStatusIs_SnapshotUpdated()
        {
            _idResolver.Setup(a => a.ResolveSnapId(It.IsAny<string>())).Returns("id");
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(true);
            _comparer.Setup(a => a.Compare(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(false);

            var newObj = new {value = 2};
            var result = _snapper.Snap("name",  newObj);

            _store.Verify(a => a.StoreSnap(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotUpdated);
            result.OldSnapshot.Should().BeEquivalentTo(_obj);
            result.NewSnapshot.Should().BeEquivalentTo(newObj);
        }

        [Fact]
        public void SnapshotMatches_ShouldUpdate_ResultStatusIs_SnapshotsMatch()
        {
            _idResolver.Setup(a => a.ResolveSnapId(It.IsAny<string>())).Returns("id");
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(_obj);
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(true);
            _comparer.Setup(a => a.Compare(It.IsAny<object>(), It.IsAny<object>()))
                .Returns(true);

            var result = _snapper.Snap("name",  _obj);

            _store.Verify(a => a.StoreSnap(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotsMatch);
            result.OldSnapshot.Should().BeEquivalentTo(_obj);
            result.NewSnapshot.Should().BeEquivalentTo(_obj);
        }

        [Fact]
        public void SnapshotDoesNotExist_ShouldUpdate_ResultStatusIs_SnapshotUpdated()
        {
            _idResolver.Setup(a => a.ResolveSnapId(It.IsAny<string>())).Returns("id");
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(null);
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(true);

            var result = _snapper.Snap("name",  _obj);

            _store.Verify(a => a.StoreSnap(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
            result.Status.Should().BeEquivalentTo(SnapResultStatus.SnapshotUpdated);
            result.OldSnapshot.Should().BeNull();
            result.NewSnapshot.Should().BeEquivalentTo(_obj);
        }
    }
}
