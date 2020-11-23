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
}
