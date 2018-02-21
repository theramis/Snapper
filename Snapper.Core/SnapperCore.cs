namespace Snapper.Core
{
    public class SnapperCore
    {
        private readonly IAssert _asserter;
        private readonly ISnapStore _store;
        private readonly IPathResolver _pathResolver;
        private readonly ISnapUpdateDecider _snapUpdateDecider;
        private readonly ISnapComparer _comparer;

        public SnapperCore(IAssert asserter, ISnapStore store, IPathResolver resolver, ISnapUpdateDecider snapUpdateDecider, ISnapComparer comparer)
        {
            _asserter = asserter;
            _store = store;
            _pathResolver = resolver;
            _snapUpdateDecider = snapUpdateDecider;
            _comparer = comparer;
        }

        public void Snap(string snapshotName, object newSnapValue)
        {
            var path = _pathResolver.ResolvePath(snapshotName);
            var oldSnapshot = _store.GetSnap(path);

            if (oldSnapshot != null)
            {
                AssertSnapshot(path, newSnapValue, oldSnapshot);
                return;
            }

            if (_snapUpdateDecider.ShouldUpdateSnap())
            {
                _store.StoreSnap(path, newSnapValue);
                _asserter.AssertEqual();
                return;
            }

            _asserter.AssertNotEqual("A snapshot for this does not exist yet.");
        }

        private void AssertSnapshot(string path, object newSnapValue, object oldSnapshot)
        {
            if (_comparer.Compare(oldSnapshot, newSnapValue))
            {
                _asserter.AssertEqual();
                return;
            }

            if (_snapUpdateDecider.ShouldUpdateSnap())
            {
                _store.StoreSnap(path, newSnapValue);
                _asserter.AssertEqual();
                return;
            }

            _asserter.AssertNotEqual(oldSnapshot, newSnapValue);
        }
    }
}
