using System.IO;

namespace Snapper.Core;

internal class SnapshotId
{
    public string FilePath { get; }

    public string? PrimaryId { get; }

    public string? SecondaryId { get; }

    public SnapshotId(
        string snapshotDirectory,
        string snapshotFileName,
        string testName,
        bool storeSnapshotsPerClass,
        string? childSnapshotName = null)
    {
        FilePath = Path.Combine(snapshotDirectory, $"{snapshotFileName}.json");

        if (storeSnapshotsPerClass)
        {
            PrimaryId = testName;
            SecondaryId = childSnapshotName;
        }
        else
        {
            PrimaryId = childSnapshotName;
        }
    }
}
