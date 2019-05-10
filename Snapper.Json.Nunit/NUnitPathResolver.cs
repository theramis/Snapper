using System.IO;
using Snapper.Core;

namespace Snapper.Json.Nunit
{
    internal class NUnitPathResolver : IPathResolver
    {
        public string ResolvePath(string snapshotName)
        {
            var info = NUnitTestHelper.GetCallingTestInfo();
            var directory = Path.GetDirectoryName(info.FileName);
            var className = info.Method.DeclaringType?.Name;
            var snapName = string.IsNullOrWhiteSpace(snapshotName)
                ? $"{className}{'_'}{info.Method.Name}"
                : $"{className}{'_'}{snapshotName}";

            return Path.Combine(directory, "_snapshots", $"{snapName}.json");
        }
    }
}
