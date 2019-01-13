using System.IO;
using System.Security;
using Snapper.Core;

namespace Snapper.Json.NUnit
{
    internal class NUnitPathResolver : IPathResolver
    {
        public string ResolvePath(string snapshotName)
        {
            var (method, filePath) = NUnitTestHelper.GetCallingTestInfo();
            var directory = Path.GetDirectoryName(filePath);
            var className = method.DeclaringType?.Name;
            var snapName = string.IsNullOrWhiteSpace(snapshotName)
                ? $"{className}{'_'}{method.Name}"
                : $"{className}{'_'}{snapshotName}";

            return Path.Combine(directory, "_snapshots", $"{snapName}.json");
        }
    }
}
