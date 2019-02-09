using System;

namespace Snapper.Core
{
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