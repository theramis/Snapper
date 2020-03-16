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

        public SnapshotId ResolveSnapshotId(string childSnapshotName)
        {
            var testMethod = _testMethodResolver.ResolveTestMethod();
            var testBaseMethod = testMethod.BaseMethod;

            var directory = Path.GetDirectoryName(testMethod.FileName);
            var className = testBaseMethod.DeclaringType?.Name;

            var snapshotDirectory = Path.Combine(directory, SnapshotsDirectory);
            var storeSnapshotsPerClass = ShouldStoreSnapshotsPerClass(testBaseMethod);

            return new SnapshotId(
                snapshotDirectory,
                className,
                testMethod.MethodName,
                childSnapshotName,
                storeSnapshotsPerClass);
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
