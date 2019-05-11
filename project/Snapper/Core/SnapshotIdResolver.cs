using System.IO;
using System.Linq;
using System.Reflection;
using Snapper.Attributes;
using Snapper.Core.TestMethodResolver;

namespace Snapper.Core
{
    internal class SnapshotIdResolver
    {
        private readonly ITestMethodResolver _testMethodResolver;
        private const string SnapshotsDirectory = "_snapshots";

        public SnapshotIdResolver(ITestMethodResolver testMethodResolver)
        {
            _testMethodResolver = testMethodResolver;
        }

        public SnapshotId ResolveSnapshotId(string partialSnapshotName)
        {
            var testMethod = _testMethodResolver.ResolveTestMethod();
            var testBaseMethod = testMethod.BaseMethod;

            var directory = Path.GetDirectoryName(testMethod.FileName);
            var className = testBaseMethod.DeclaringType?.Name;

            var storeSnapshotsPerClass = ShouldStoreSnapshotsPerClass(testBaseMethod);

            if (storeSnapshotsPerClass)
            {
                var snapshotFilePath = Path.Combine(directory, SnapshotsDirectory, $"{className}.json");
                return new SnapshotId(snapshotFilePath, testMethod.MethodName, partialSnapshotName);
            }
            else
            {
                var snapshotFileName = $"{className}{'_'}{testMethod.MethodName}";
                var snapshotFilePath = Path.Combine(directory, SnapshotsDirectory, $"{snapshotFileName}.json");
                return new SnapshotId(snapshotFilePath, partialSnapshotName);
            }
        }

        private static bool ShouldStoreSnapshotsPerClass(MemberInfo method)
        {
            var classHasAttribute = HasStoreSnapshotsPerClassAttribute(method?.DeclaringType);
            var assemblyHasAttribute = HasStoreSnapshotsPerClassAttribute(method?.DeclaringType?.Assembly);

            return classHasAttribute || assemblyHasAttribute;
        }

        private static bool HasStoreSnapshotsPerClassAttribute(ICustomAttributeProvider member)
            => member?.GetCustomAttributes(typeof(StoreSnapshotsPerClassAttribute), true).Any() ?? false;
    }
}
