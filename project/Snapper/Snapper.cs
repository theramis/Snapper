using System;
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
            => MatchSnapshot(snapshot, null);

        public void MatchSnapshot(object snapshot, string snapshotName)
        {
            var snapId = _snapshotIdResolver.ResolveSnapshotId(null);
            var sanitisedSnapshot = _snapshotSanitiser.SanitiseSnapshot(snapshot);

            var result = Snap(snapId, sanitisedSnapshot);

            SnapshotAsserter.AssertSnapshot(result);
        }
    }
}
