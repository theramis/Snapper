using System.Collections.Generic;
using Snapper.Attributes;
using Snapper.Core.TestMethodResolver;

namespace Snapper.Core
{
    internal class CoreSnapshotHandler : ISnapshotHandler
    {
        private readonly ISnapshotStore _snapshotStore;
        private readonly ISnapshotComparer _snapshotComparer;
        private readonly ISnapshotUpdateDecider _snapshotUpdateDecider;

        public CoreSnapshotHandler(
            ISnapshotStore snapshotStore,
            ISnapshotComparer snapshotComparer,
            ISnapshotUpdateDecider snapshotUpdateDecider)
        {
            _snapshotStore = snapshotStore;
            _snapshotComparer = snapshotComparer;
            _snapshotUpdateDecider = snapshotUpdateDecider;
        }

        public SnapResult Snap(SnapshotId id, object newSnapshot)
        {
            var currentSnapshot = _snapshotStore.GetSnapshot(id);
            var result = _snapshotComparer.CompareSnapshots(currentSnapshot, newSnapshot);
            if (!ShouldUpdate(result)) return result;
            _snapshotStore.StoreSnapshot(id, newSnapshot);
            return SnapResult.SnapshotUpdated(currentSnapshot, newSnapshot);
        }

        private bool ShouldUpdate(SnapResult result)
        {
            switch (result.Status)
            {
                case SnapResultStatus.SnapshotDoesNotExist:
                case SnapResultStatus.SnapshotsDoNotMatch: return _snapshotUpdateDecider.ShouldUpdateSnapshot();
                default: return false;
            }
        }
    }

    internal class PostSnapshotHandler : ISnapshotHandler
    {
        private readonly ISnapshotHandler _snapshotHandler;
        private readonly ITestMethodResolver _testMethodResolver;

        public PostSnapshotHandler(
            ISnapshotHandler snapshotHandler,
            ITestMethodResolver testMethodResolver)
        {
            _snapshotHandler = snapshotHandler;
            _testMethodResolver = testMethodResolver;
        }

        public SnapResult Snap(SnapshotId id, object newSnapshot)
        {
            var result = _snapshotHandler.Snap(id, newSnapshot);

            foreach (var attr in GetAttributes()) attr.Handle(result);

            return result;
        }

        private IEnumerable<PostSnapshotAttribute> GetAttributes()
        {
            var method = _testMethodResolver.ResolveTestMethod().BaseMethod;
            return method.GetCustomAttributes<PostSnapshotAttribute>();
        }
    }
}
