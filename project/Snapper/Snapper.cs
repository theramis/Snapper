using Snapper.Core;
using Snapper.Json;

namespace Snapper
{
    internal class Snapper
    {
        private readonly SnapshotIdResolver _snapshotIdResolver;
        private readonly JsonSnapshotSanitiser _snapshotSanitiser;
        private readonly SnapshotAsserter _snapshotAsserter;
        private readonly ISnapshotHandler _snapshotHandler;

        public Snapper(
            SnapshotIdResolver snapshotIdResolver,
            JsonSnapshotSanitiser snapshotSanitiser,
            SnapshotAsserter snapshotAsserter,
            ISnapshotHandler snapshotHandler)
        {
            _snapshotIdResolver = snapshotIdResolver;
            _snapshotSanitiser = snapshotSanitiser;
            _snapshotAsserter = snapshotAsserter;
            _snapshotHandler = snapshotHandler;
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
            var result = _snapshotHandler.Snap(snapshotId, sanitisedSnapshot);
            _snapshotAsserter.AssertSnapshot(result);
        }
    }
}
