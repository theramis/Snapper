using System.IO;
using System.Linq;
using System.Reflection;
using Snapper.Attributes;

namespace Snapper.Core
{
    internal class SnapshotIdResolver : ISnapshotIdResolver
    {
        private const string SnapshotsDirectory = "_snapshots";

        public SnapshotId ResolveSnapshotId(string snapshotName)
        {
            var (method, filePath) = TestFrameworkHelper.GetCallingTestMethod();

            var storeSnapshotsPerClass = ShouldStoreSnapshotsPerClass(method);

            var directory = Path.GetDirectoryName(filePath);
            var className = method.DeclaringType?.Name;

            if (storeSnapshotsPerClass)
            {
                var snapshotFilePath = Path.Combine(directory, SnapshotsDirectory, $"{className}.json");
                var snapshotId = string.IsNullOrWhiteSpace(snapshotName) ? method.Name : snapshotName;
                return new SnapshotId(snapshotFilePath, snapshotId);
            }
            else
            {
                var snapshotFileName = string.IsNullOrWhiteSpace(snapshotName)
                                            ? $"{className}{'_'}{method.Name}"
                                            : $"{className}{'_'}{snapshotName}";

                var snapshotFilePath = Path.Combine(directory, SnapshotsDirectory, $"{snapshotFileName}.json");
                return new SnapshotId(snapshotFilePath);
            }
        }

        private static bool ShouldStoreSnapshotsPerClass(MemberInfo method)
        {
            var methodHasAttribute = HasStoreSnapshotsPerClassAttribute(method);
            var classHasAttribute = HasStoreSnapshotsPerClassAttribute(method?.ReflectedType);
            var assemblyHasAttribute = HasStoreSnapshotsPerClassAttribute(method?.ReflectedType?.Assembly);

            return methodHasAttribute || classHasAttribute || assemblyHasAttribute;
        }

        private static bool HasStoreSnapshotsPerClassAttribute(ICustomAttributeProvider member)
            => member?.GetCustomAttributes(typeof(StoreSnapshotsPerClassAttribute), true).Any() ?? false;
    }
}
