using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Snapper.Attributes;
using Snapper.Core.TestMethodResolver;

namespace Snapper.Core;

internal class SnapshotUpdateDecider : ISnapshotUpdateDecider
{
    private const string UpdateSnapshotEnvironmentVariableName = "UpdateSnapshots";
    private readonly ITestMethodResolver _testMethodResolver;
    private readonly string _envVarName;
    private readonly SnapshotSettings? _settings;

    public SnapshotUpdateDecider(ITestMethodResolver testMethodResolver,
        string envVarName = UpdateSnapshotEnvironmentVariableName,
        SnapshotSettings? settings = null)
    {
        _testMethodResolver = testMethodResolver;
        _envVarName = envVarName;
        _settings = settings;
    }

    public bool ShouldUpdateSnapshot()
        => ShouldUpdateSnapshotBasedOnSettings() ?? 
        (ShouldUpdateSnapshotBasedOnEnvironmentVariable() 
        || ShouldUpdateSnapshotBasedOnAttribute());

    private bool? ShouldUpdateSnapshotBasedOnSettings()
        => _settings?.ShouldUpdateSnapshots;

    private bool ShouldUpdateSnapshotBasedOnEnvironmentVariable()
    {
        var env = Environment.GetEnvironmentVariable(_envVarName);
        if (env == null)
            return false;
        return bool.TryParse(env, out var value) && value;
    }

    private bool ShouldUpdateSnapshotBasedOnAttribute()
    {
        var method = _testMethodResolver.ResolveTestMethod().BaseMethod;

        var customAttributeProviders = new List<ICustomAttributeProvider?>
        {
            method, // check method
            method.DeclaringType, // check class
            method.DeclaringType?.Assembly // check assembly
        };

        foreach (var customAttributeProvider in customAttributeProviders)
        {
            if (TryGetUpdateSnapshotsAttribute(customAttributeProvider, out var att))
            {
                if (att == null)
                {
                    return false;
                }
                return !(att.IgnoreIfCi && CiEnvironmentDetector.IsCiEnv());
            }
        }

        return false;
    }

    private static bool TryGetUpdateSnapshotsAttribute(ICustomAttributeProvider? member, out UpdateSnapshotsAttribute? attribute)
    {
        var attributes = member?.GetCustomAttributes(typeof(UpdateSnapshotsAttribute), true);

        attribute = attributes?.FirstOrDefault() as UpdateSnapshotsAttribute;
        return attribute != null;
    }
}
