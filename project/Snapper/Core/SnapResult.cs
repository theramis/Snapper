using Snapper.Exceptions;
using Snapper.Json;

namespace Snapper.Core;

internal class SnapResult
{
    public SnapResultStatus Status { get; }
    public JsonSnapshot? OldSnapshot { get; }
    public JsonSnapshot NewSnapshot { get; }

    private SnapResult(SnapResultStatus status, JsonSnapshot? oldSnapshot, JsonSnapshot newSnapshot)
    {
        Status = status;
        OldSnapshot = oldSnapshot;
        NewSnapshot = newSnapshot;
    }

    public static SnapResult SnapshotDoesNotExist(JsonSnapshot newSnapshot)
        => new SnapResult(SnapResultStatus.SnapshotDoesNotExist, null, newSnapshot);

    public static SnapResult SnapshotsMatch(JsonSnapshot oldSnapshot, JsonSnapshot newSnapshot)
        => new SnapResult(SnapResultStatus.SnapshotsMatch, oldSnapshot, newSnapshot);

    public static SnapResult SnapshotsDoNotMatch(JsonSnapshot oldSnapshot, JsonSnapshot newSnapshot)
        => new SnapResult(SnapResultStatus.SnapshotsDoNotMatch, oldSnapshot, newSnapshot);

    public static SnapResult SnapshotUpdated(JsonSnapshot? oldSnapshot, JsonSnapshot newSnapshot)
        => new SnapResult(SnapResultStatus.SnapshotUpdated, oldSnapshot, newSnapshot);

    public void AssertSnapshot()
    {
        switch (Status)
        {
            case SnapResultStatus.SnapshotDoesNotExist:
                throw new SnapshotDoesNotExistException(this);
            case SnapResultStatus.SnapshotsDoNotMatch:
                throw new SnapshotsDoNotMatchException(this);
            case SnapResultStatus.SnapshotUpdated:
            case SnapResultStatus.SnapshotsMatch:
            default:
                return;
        }
    }
}
