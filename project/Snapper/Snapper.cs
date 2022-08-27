using Snapper.Core;
using Snapper.Json;

namespace Snapper
{
    internal class Snapper : SnapperCore
    {
        private readonly SnapshotIdResolver _snapshotIdResolver;
        private readonly JsonSnapshotSanitiser _snapshotSanitiser;

        public Snapper(ISnapshotStore snapshotStore, ISnapshotUpdateDecider snapshotUpdateDecider,
            ISnapshotComparer snapshotComparer, SnapshotIdResolver snapshotIdResolver,
            JsonSnapshotSanitiser snapshotSanitiser)
            : base(snapshotStore, snapshotUpdateDecider, snapshotComparer)
        {
            _snapshotIdResolver = snapshotIdResolver;
            _snapshotSanitiser = snapshotSanitiser;
        }

        public SnapResult MatchSnapshot(object snapshot)
            => MatchSnapshot(snapshot, childSnapshotName: null);

        public SnapResult MatchSnapshot(object snapshot, string childSnapshotName)
        {
            var snapshotId = _snapshotIdResolver.ResolveSnapshotId(childSnapshotName);
            return MatchSnapshot(snapshot, snapshotId);
        }

        public SnapResult MatchSnapshot(object snapshot, SnapshotId snapshotId)
        {
            var sanitisedSnapshot = _snapshotSanitiser.SanitiseSnapshot(snapshot);
            return Snap(snapshotId, sanitisedSnapshot);
        }
    }
}
