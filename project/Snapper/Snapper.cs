using Snapper.Core;
using Snapper.Json;

namespace Snapper;

internal class Snapper : SnapperCore
{
    private readonly SnapshotIdResolver _snapshotIdResolver;
    private readonly JsonSnapshotSanitiser _snapshotSanitiser;
    private readonly SnapshotSettings _snapshotSettings;

    public Snapper(ISnapshotStore snapshotStore, ISnapshotUpdateDecider snapshotUpdateDecider,
        SnapshotIdResolver snapshotIdResolver, JsonSnapshotSanitiser snapshotSanitiser,
        SnapshotSettings snapshotSettings)
        : base(snapshotStore, snapshotUpdateDecider)
    {
        _snapshotIdResolver = snapshotIdResolver;
        _snapshotSanitiser = snapshotSanitiser;
        _snapshotSettings = snapshotSettings;
    }

    public SnapResult MatchSnapshot(object snapshot)
        => MatchSnapshot(snapshot, childSnapshotName: null);

    public SnapResult MatchSnapshot(object snapshot, string? childSnapshotName)
    {
        var snapshotId = _snapshotIdResolver.ResolveSnapshotId(childSnapshotName, _snapshotSettings);
        return MatchSnapshot(snapshot, snapshotId);
    }

    public SnapResult MatchSnapshot(object rawSnapshot, SnapshotId snapshotId)
    {
        var sanitisedSnapshot = _snapshotSanitiser.SanitiseSnapshot(rawSnapshot);
        var snapshot = new JsonSnapshot(snapshotId, sanitisedSnapshot);
        return Snap(snapshot);
    }
}
