using System.IO;

namespace Snapper.Core
{
    public class SnapshotId
    {
        internal string FilePath { get; }

        internal string PrimaryId { get; }

        internal string SecondaryId { get; }

        /// <summary>
        ///     Describes how and where the snapshot will be stored.
        ///     See <see href="https://theramis.github.io/Snapper/#/pages/snapper/advanced_snapshot_control">Advanced snapshot file control</see>
        /// </summary>
        /// <param name="snapshotDirectory">The directory where the snapshot should be stored</param>
        /// <param name="className">The name of the class where the test is defined</param>
        /// <param name="methodName">The name of the test method</param>
        /// <param name="childSnapshotName">The name of the child snapshot. Use null if there none</param>
        /// <param name="storeSnapshotsPerClass">Set to true to store snapshot per class.</param>
        public SnapshotId(string snapshotDirectory,
            string className,
            string methodName,
            string childSnapshotName = null,
            bool storeSnapshotsPerClass = false)
        {
            if (storeSnapshotsPerClass)
            {
                FilePath = Path.Combine(snapshotDirectory, $"{className}.json");
                PrimaryId = methodName;
                SecondaryId = childSnapshotName;
            }
            else
            {
                FilePath = Path.Combine(snapshotDirectory, $"{className}{'_'}{methodName}.json");
                PrimaryId = childSnapshotName;
            }
        }
    }
}
