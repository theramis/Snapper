using System;
using Snapper.Core;

namespace Snapper
{
    internal class Snapper : SnapperCore
    {
        private readonly ISnapshotIdResolver _snapshotIdResolver;
        private readonly ISnapshotSanitiser _snapshotSanitiser;
        private readonly ISnapshotAsserter _snapshotAsserter;

        public Snapper(ISnapshotStore snapshotStore, ISnapshotUpdateDecider snapshotUpdateDecider,
            ISnapshotComparer snapshotComparer, ISnapshotIdResolver snapshotIdResolver,
            ISnapshotSanitiser snapshotSanitiser, ISnapshotAsserter snapshotAsserter)
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

            _snapshotAsserter.AssertSnapshot(result);
        }
    }
}
