using System;
using System.Linq;
using System.Reflection;
using Snapper.Attributes;
using Snapper.Core.TestFrameworks;

namespace Snapper.Core
{
    // TODO write tests for this class
    internal class SnapshotUpdateDecider : ISnapshotUpdateDecider
    {
        private const string UpdateSnapshotEnvironmentVariableName = "UpdateSnapshots";
        private readonly string _envVarName;

        public SnapshotUpdateDecider()
            : this(UpdateSnapshotEnvironmentVariableName)
        {
        }

        public SnapshotUpdateDecider(string envVarName)
            => _envVarName = envVarName;

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

        private static bool ShouldUpdateSnapshotBasedOnAttribute()
        {
            var (method, _) = TestFrameworkHelper.GetCallingTestMethod();

            var methodHasAttribute = HasUpdateSnapshotsAttribute(method);
            var classHasAttribute = HasUpdateSnapshotsAttribute(method?.ReflectedType);
            var assemblyHasAttribute = HasUpdateSnapshotsAttribute(method?.ReflectedType?.Assembly);

            return methodHasAttribute || classHasAttribute || assemblyHasAttribute;
        }

        private static bool HasUpdateSnapshotsAttribute(ICustomAttributeProvider member)
            => member?.GetCustomAttributes(typeof(UpdateSnapshotsAttribute), true).Any() ?? false;
    }
}