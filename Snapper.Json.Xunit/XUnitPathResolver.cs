using System.IO;
using Snapper.Core;

namespace Snapper.Json.Xunit
{
    internal class XUnitPathResolver : IPathResolver
    {
        public string ResolvePath(string snapshotName)
        {
            var (method, filePath) = XUnitTestHelper.GetCallingTestInfo();
            var directory = Path.GetDirectoryName(filePath);
            var snapName = string.IsNullOrWhiteSpace(snapshotName) ? method.Name : snapshotName;

            return Path.Combine(directory, "_snapshots", $"{snapName}.json");
        }
    }
}
