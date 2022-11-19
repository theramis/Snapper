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


    // tests needed
    /*
    - inside class with store snapshots as class
         - resolve based on snapshotsettings fully
            - child
            - normal
            - test which one is applied

    partial tests needed?
     */
    public SnapshotId ResolveSnapshotId(string? childSnapshotName, SnapshotSettings snapshotSettings)
    {
        var testMethod = new Lazy<ITestMethod>(() => _testMethodResolver.ResolveTestMethod());

        var storeSnapshotsPerClass = snapshotSettings.ShouldStoreSnapshotsPerClass ??
                                     ShouldStoreSnapshotsPerClass(testMethod.Value);
        var snapshotDirectory = snapshotSettings.Directory ?? GetSnapshotDirectory(testMethod.Value);

        var className = new Lazy<string?>(() =>
            snapshotSettings.ClassName ?? testMethod.Value.BaseMethod.DeclaringType?.Name);
        var testName = new Lazy<string?>(() =>
            snapshotSettings.TestName ?? testMethod.Value.MethodName);

        if (storeSnapshotsPerClass)
        {
            var fileName = snapshotSettings.FileName ?? $"{className.Value}";
            var filePath = Path.Combine(snapshotDirectory, $"{fileName}.json");
            return new SnapshotId(filePath, testName.Value, childSnapshotName);
        }
        else
        {
            var fileName = snapshotSettings.FileName ?? $"{className.Value}{'_'}{testName.Value}";
            var filePath = Path.Combine(snapshotDirectory, $"{fileName}.json");
            return new SnapshotId(filePath, childSnapshotName, null);
        }
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
}
