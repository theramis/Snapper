using System;
using Snapper.Core;

namespace Snapper
{
    internal class Snapper : SnapperCore
    {
        private readonly ISnapshotIdResolver _snapshotIdResolver;

        public Snapper(ISnapshotStore snapshotStore, ISnapshotUpdateDecider snapshotUpdateDecider,
            ISnapshotComparer snapshotComparer, ISnapshotIdResolver snapshotIdResolver)
            : base(snapshotStore, snapshotUpdateDecider, snapshotComparer)
        {
            _snapshotIdResolver = snapshotIdResolver;
        }

        public void MatchSnapshot(object snapshot)
            => MatchSnapshot(snapshot, null);

        public void MatchSnapshot(object snapshot, string snapshotName)
        {
            var snapId = _snapshotIdResolver.ResolveSnapshotId(null);

            // TODO sanitise object
            // e.g. not convertable to a json object try convert

            var result = Snap(snapId, snapshot);

            // TODO call asserter
            if (result.Status != SnapResultStatus.SnapshotsMatch || result.Status == SnapResultStatus.SnapshotUpdated)
                throw new Exception("Break break break");
        }
    }
}
