using Snapper.Core;
using Snapper.Json;

namespace Snapper.Nunit
{
    internal class NUnitSnapper : SnapperCore
    {
        private readonly SnapshotIdResolver _snapshotIdResolver;
        private readonly JsonSnapshotSanitiser _snapshotSanitiser;

        public NUnitSnapper(ISnapshotStore snapshotStore, ISnapshotUpdateDecider snapshotUpdateDecider,
            ISnapshotComparer snapshotComparer, SnapshotIdResolver snapshotIdResolver,
            JsonSnapshotSanitiser snapshotSanitiser)
            : base(snapshotStore, snapshotUpdateDecider, snapshotComparer)
        {
            _snapshotIdResolver = snapshotIdResolver;
            _snapshotSanitiser = snapshotSanitiser;
        }

        public SnapResult MatchSnapshot(object snapshot)
            => MatchSnapshot(snapshot, null);

        public SnapResult MatchSnapshot(object snapshot, string partialSnapshotName)
        {
            var snapId = _snapshotIdResolver.ResolveSnapshotId(partialSnapshotName);
            var sanitisedSnapshot = _snapshotSanitiser.SanitiseSnapshot(snapshot);

            return Snap(snapId, sanitisedSnapshot);
        }
    }
}
