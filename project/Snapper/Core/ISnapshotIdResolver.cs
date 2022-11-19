namespace Snapper.Core;

internal interface ISnapshotIdResolver
{
    SnapshotId ResolveSnapshotId(string? childSnapshotName, SnapshotSettings snapshotSettings);
}
