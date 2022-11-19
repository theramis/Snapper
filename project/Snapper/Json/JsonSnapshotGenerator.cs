using Snapper.Core;

namespace Snapper.Json;

internal class JsonSnapshotGenerator
{
    private readonly ISnapshotIdResolver _snapshotIdResolver;
    private readonly JsonSnapshotSanitiser _snapshotSanitiser;
    private readonly SnapshotSettings _snapshotSettings;

    public JsonSnapshotGenerator(ISnapshotIdResolver snapshotIdResolver,
        JsonSnapshotSanitiser jsonSnapshotSanitiser, SnapshotSettings snapshotSettings)
    {
        _snapshotIdResolver = snapshotIdResolver;
        _snapshotSanitiser = jsonSnapshotSanitiser;
        _snapshotSettings = snapshotSettings;
    }

    public JsonSnapshot Generate(object rawSnapshot, string? childSnapshotName)
    {
        var snapshotId = _snapshotIdResolver.ResolveSnapshotId(childSnapshotName, _snapshotSettings);
        var sanitisedSnapshot = _snapshotSanitiser.SanitiseSnapshot(rawSnapshot);
        return new JsonSnapshot(snapshotId, sanitisedSnapshot);
    }
}
