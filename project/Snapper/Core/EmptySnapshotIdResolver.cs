namespace Snapper.Core;

internal class EmptySnapshotIdResolver : ISnapshotIdResolver
{
    public SnapshotId ResolveSnapshotId(string? childSnapshotName, SnapshotSettings snapshotSettings)
        => new SnapshotId(string.Empty, null, null);
}
