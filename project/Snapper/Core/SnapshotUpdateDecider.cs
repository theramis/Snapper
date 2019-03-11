using System;
using System.Linq;
using System.Reflection;
using Snapper.Attributes;
using Snapper.Core.TestMethodResolver;

namespace Snapper.Core
{
    // TODO write tests for this class
    internal class SnapshotUpdateDecider : ISnapshotUpdateDecider
    {
        private readonly ITestMethodResolver _testMethodResolver;
        private const string UpdateSnapshotEnvironmentVariableName = "UpdateSnapshots";
        private readonly string _envVarName;

        public SnapshotUpdateDecider(ITestMethodResolver testMethodResolver,
            string envVarName = UpdateSnapshotEnvironmentVariableName)
        {
            _testMethodResolver = testMethodResolver;
            _envVarName = envVarName;
        }

        public bool ShouldUpdateSnapshot()
            => ShouldUpdateSnapshotBasedOnEnvironmentVariable()
                   || ShouldUpdateSnapshotBasedOnAttribute();

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

            var methodHasAttribute = HasUpdateSnapshotsAttribute(method);
            var classHasAttribute = HasUpdateSnapshotsAttribute(method?.ReflectedType);
            var assemblyHasAttribute = HasUpdateSnapshotsAttribute(method?.ReflectedType?.Assembly);

            return methodHasAttribute || classHasAttribute || assemblyHasAttribute;
        }

        private static bool HasUpdateSnapshotsAttribute(ICustomAttributeProvider member)
            => member?.GetCustomAttributes(typeof(UpdateSnapshotsAttribute), true).Any() ?? false;
    }
}