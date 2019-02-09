using System;

namespace Snapper.Core
{
    // TODO Make this look for the Update Snapshot attribute in assembly or class or method
    public class EnvironmentVariableUpdateDecider : ISnapshotUpdateDecider
    {
        private const string UpdateSnapshotEnvironmentVariableName = "UpdateSnapshots";
        private readonly string _envVarName;

        public EnvironmentVariableUpdateDecider()
            : this(UpdateSnapshotEnvironmentVariableName)
        {
        }

        public EnvironmentVariableUpdateDecider(string envVarName)
            => _envVarName = envVarName;

        public bool ShouldUpdateSnapshot()
        {
            var env = Environment.GetEnvironmentVariable(_envVarName);
            if (env == null)
                return false;
            return bool.TryParse(env, out var value) && value;
        }
    }
}