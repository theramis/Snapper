using Moq;
using Xunit;

namespace Snapper.Core.Tests
{
    public class SnapperCoreTests
    {
        private readonly object _obj = new {value = 1};
        private readonly SnapperCore _snapper;
        private readonly Mock<IAssert> _asserter;
        private readonly Mock<ISnapStore> _store;
        private readonly Mock<IPathResolver> _pathResolver;
        private readonly Mock<ISnapUpdateDecider> _updateDecider;
        private readonly Mock<ISnapComparer> _comparer;

        public SnapperCoreTests()
        {
            _asserter = new Mock<IAssert>();
            _store = new Mock<ISnapStore>();
            _pathResolver = new Mock<IPathResolver>();
            _updateDecider = new Mock<ISnapUpdateDecider>();
            _comparer = new Mock<ISnapComparer>();

            _snapper = new SnapperCore(_asserter.Object, _store.Object, _pathResolver.Object,
                _updateDecider.Object, _comparer.Object);
        }

        [Fact]
        public void SnapshotDoesNotExist_AssertNotEqual()
        {
            _pathResolver.Setup(a => a.ResolvePath(It.IsAny<string>())).Returns("path");
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(false);
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(null);

            _snapper.Snap("name", _obj);

            _asserter.Verify(a => a.AssertNotEqual(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SnapshotMatches_AssertEqual()
        {
            _pathResolver.Setup(a => a.ResolvePath(It.IsAny<string>())).Returns("path");
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(false);
            _comparer.Setup(a => a.Compare(It.IsAny<object>(), It.IsAny<object>())).Returns(true);
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(_obj);

            _snapper.Snap("name", _obj);

            _asserter.Verify(a => a.AssertEqual(), Times.Once);
        }

        [Fact]
        public void SnapshotDoesNotMatch_AssertNotEqual()
        {
            _pathResolver.Setup(a => a.ResolvePath(It.IsAny<string>())).Returns("path");
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(false);
            _comparer.Setup(a => a.Compare(It.IsAny<object>(), It.IsAny<object>())).Returns(false);
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(_obj);

            _snapper.Snap("name", _obj);

            _asserter.Verify(a => a.AssertNotEqual(It.IsAny<object>(), It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public void SnapshotDoesNotMatch_ShouldUpdate_Updates()
        {
            _pathResolver.Setup(a => a.ResolvePath(It.IsAny<string>())).Returns("path");
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(true);
            _comparer.Setup(a => a.Compare(It.IsAny<object>(), It.IsAny<object>())).Returns(false);
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(_obj);

            _snapper.Snap("name", _obj);

            _store.Verify(a => a.StoreSnap(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
            _asserter.Verify(a => a.AssertEqual(), Times.Once);
        }

        [Fact]
        public void SnapshotMatches_ShouldUpdate_AssertEqual()
        {
            _pathResolver.Setup(a => a.ResolvePath(It.IsAny<string>())).Returns("path");
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(true);
            _comparer.Setup(a => a.Compare(It.IsAny<object>(), It.IsAny<object>())).Returns(true);
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(_obj);

            _snapper.Snap("name", _obj);

            _store.Verify(a => a.StoreSnap(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
            _asserter.Verify(a => a.AssertEqual(), Times.Once);
        }

        [Fact]
        public void SnapshotDoesNotExist_ShouldUpdate_Updates()
        {
            _pathResolver.Setup(a => a.ResolvePath(It.IsAny<string>())).Returns("path");
            _updateDecider.Setup(a => a.ShouldUpdateSnap()).Returns(true);
            _store.Setup(a => a.GetSnap(It.IsAny<string>())).Returns(null);

            _snapper.Snap("name", _obj);

            _store.Verify(a => a.StoreSnap(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
            _asserter.Verify(a => a.AssertEqual(), Times.Once);
        }
    }
}
