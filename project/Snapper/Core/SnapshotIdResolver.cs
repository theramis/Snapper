using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Snapper.Attributes;
using Snapper.Core.TestMethodResolver;
using Snapper.Core.TestMethodResolver.TestMethods;

namespace Snapper.Core;

internal class SnapshotIdResolver
{
    private readonly ITestMethodResolver _testMethodResolver;
    private const string SnapshotsDirectory = "_snapshots";

    public SnapshotIdResolver(ITestMethodResolver testMethodResolver)
    {
        _testMethodResolver = testMethodResolver;
    }

    public SnapshotId ResolveSnapshotId(string? childSnapshotName, SnapshotSettings snapshotSettings)
    {
        var testMethod = new Lazy<ITestMethod>(() => _testMethodResolver.ResolveTestMethod());

        var storeSnapshotsPerClass = snapshotSettings.ShouldStoreSnapshotsPerClass ??
                                     ShouldStoreSnapshotsPerClass(testMethod.Value);
        var snapshotDirectory = snapshotSettings.Directory ?? GetSnapshotDirectory(testMethod.Value);
        var testName = snapshotSettings.TestName ?? testMethod.Value.MethodName;
        var snapshotFileName = snapshotSettings.FileName ??
                               GetSnapshotFileName(snapshotSettings, testMethod, testName, storeSnapshotsPerClass);

        return new SnapshotId(snapshotDirectory, snapshotFileName, testName, storeSnapshotsPerClass, childSnapshotName);
    }

    private static bool ShouldStoreSnapshotsPerClass(ITestMethod testMethod)
    {
        var method = testMethod.BaseMethod;
        var classHasAttribute = HasStoreSnapshotsPerClassAttribute(method.DeclaringType);
        var assemblyHasAttribute = HasStoreSnapshotsPerClassAttribute(method.DeclaringType?.Assembly);

        return classHasAttribute || assemblyHasAttribute;
    }

    private static bool HasStoreSnapshotsPerClassAttribute(ICustomAttributeProvider? member)
        => member?.GetCustomAttributes(typeof(StoreSnapshotsPerClassAttribute), true).Any() ?? false;

    private static string GetSnapshotDirectory(ITestMethod testMethod)
    {
        var directory = Path.GetDirectoryName(testMethod.FileName) ?? throw new InvalidOperationException();
        return Path.Combine(directory, SnapshotsDirectory);
    }

    private static string GetSnapshotFileName(SnapshotSettings snapshotSettings, Lazy<ITestMethod> testMethod,
        string testName, bool storeSnapshotsPerClass)
    {
        var className = snapshotSettings.ClassName ?? testMethod.Value.BaseMethod.DeclaringType?.Name;
        return storeSnapshotsPerClass ? $"{className}" : $"{className}{'_'}{testName}";
    }
}
