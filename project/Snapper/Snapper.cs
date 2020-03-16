using Snapper.Core;
using Snapper.Json;

namespace Snapper
{
    internal class Snapper : SnapperCore
    {
        private readonly SnapshotIdResolver _snapshotIdResolver;
        private readonly JsonSnapshotSanitiser _snapshotSanitiser;
        private readonly SnapshotAsserter _snapshotAsserter;

        public Snapper(ISnapshotStore snapshotStore, ISnapshotUpdateDecider snapshotUpdateDecider,
            ISnapshotComparer snapshotComparer, SnapshotIdResolver snapshotIdResolver,
            JsonSnapshotSanitiser snapshotSanitiser, SnapshotAsserter snapshotAsserter)
            : base(snapshotStore, snapshotUpdateDecider, snapshotComparer)
        {
            _snapshotIdResolver = snapshotIdResolver;
            _snapshotSanitiser = snapshotSanitiser;
            _snapshotAsserter = snapshotAsserter;
        }

        public void MatchSnapshot(object snapshot)
            => MatchSnapshot(snapshot, childSnapshotName: null);

        public void MatchSnapshot(object snapshot, string childSnapshotName)
        {
            var snapshotId = _snapshotIdResolver.ResolveSnapshotId(childSnapshotName);
            MatchSnapshot(snapshot, snapshotId);
        }

        public void MatchSnapshot(object snapshot, SnapshotId snapshotId)
        {
            var sanitisedSnapshot = _snapshotSanitiser.SanitiseSnapshot(snapshot);
            var result = Snap(snapshotId, sanitisedSnapshot);
            _snapshotAsserter.AssertSnapshot(result);
        }
    }
}
