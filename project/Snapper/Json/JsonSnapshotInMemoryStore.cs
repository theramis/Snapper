using Snapper.Core;

namespace Snapper.Json
{
    internal class JsonSnapshotInMemoryStore : ISnapshotStore
    {
        private readonly object _snapshot;

        public JsonSnapshotInMemoryStore(JsonSnapshotSanitiser jsonSnapshotSanitiser, object snapshot)
        {
            _snapshot = jsonSnapshotSanitiser.SanitiseSnapshot(snapshot);
        }

        public object GetSnapshot(SnapshotId snapshotId)
            => _snapshot;

        public void StoreSnapshot(SnapshotId snapshotId, object snapshot)
        {}
    }
}
