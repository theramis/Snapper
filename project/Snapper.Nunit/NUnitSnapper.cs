using Snapper.Core;
using Snapper.Json;

namespace Snapper.Nunit
{
    internal class NUnitSnapper
    {
        private readonly SnapshotIdResolver _snapshotIdResolver;
        private readonly JsonSnapshotSanitiser _snapshotSanitiser;
        private readonly ISnapshotHandler _snapshotHandler;

        public NUnitSnapper(
            SnapshotIdResolver snapshotIdResolver,
            JsonSnapshotSanitiser snapshotSanitiser,
            ISnapshotHandler snapshotHandler)
        {
            _snapshotIdResolver = snapshotIdResolver;
            _snapshotSanitiser = snapshotSanitiser;
            _snapshotHandler = snapshotHandler;
        }

        public SnapResult MatchSnapshot(object snapshot)
            => MatchChildSnapshot(snapshot, null);

        public SnapResult MatchChildSnapshot(object snapshot, string childSnapshotName)
        {
            var snapshotId = _snapshotIdResolver.ResolveSnapshotId(childSnapshotName);
            return MatchSnapshot(snapshot, snapshotId);
        }

        public SnapResult MatchSnapshot(object snapshot, SnapshotId snapshotId)
        {
            var sanitisedSnapshot = _snapshotSanitiser.SanitiseSnapshot(snapshot);
            return _snapshotHandler.Snap(snapshotId, sanitisedSnapshot);
        }
    }
}
