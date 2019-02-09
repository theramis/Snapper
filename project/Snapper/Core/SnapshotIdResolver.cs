namespace Snapper.Core
{
    public class SnapshotIdResolver : ISnapshotIdResolver
    {
        public string ResolveSnapshotId(string snapshotName)
        {

            // get method where ShouldMatchSnapshot is called
            // get file path of class/method

            // determine whether it snapshots per class is enabled
            // search for both assembly and class
            throw new System.NotImplementedException();
        }
    }
}
