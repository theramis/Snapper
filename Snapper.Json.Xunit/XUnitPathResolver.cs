using System.IO;
using Snapper.Core;

namespace Snapper.Json.Xunit
{
    public class XUnitPathResolver : IPathResolver
    {
        public string ResolvePath(string snapshotName)
        {
            var info = XUnitTestHelper.GetCallingTestInfo();
            var directory = Path.GetDirectoryName(info.FileName);
            var snapName = string.IsNullOrWhiteSpace(snapshotName) ? info.Method.Name : snapshotName;

            return Path.Combine(directory, "_snapshots", $"{snapName}.json");
        }
    }
}
