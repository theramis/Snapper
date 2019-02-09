using System.IO;

namespace Snapper.Core
{
    public class SnapshotIdResolver : ISnapshotIdResolver
    {
        private const string SnapshotsDirectory = "_snapshots";

        public string ResolveSnapshotId(string snapshotName)
        {
            var (method, filePath) = TestFrameworkHelper.GetCallingTestMethod();

            var directory = Path.GetDirectoryName(filePath);
            var snapName = string.IsNullOrWhiteSpace(snapshotName) ? method.Name : snapshotName;

            // TODO add stuff to make snapshots per class
            // determine whether it snapshots per class is enabled
            // search for both assembly and class

            return Path.Combine(directory, "_snapshots", $"{snapName}.json");
        }
    }
}
