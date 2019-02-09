using System;

namespace Snapper.Core
{
    public sealed class EnvironmentVariableUpdateDecider : ISnapUpdateDecider
    {
        private readonly string _envVarName;

        public EnvironmentVariableUpdateDecider(string envVarName = "UpdateSnapshots")
        {
            _envVarName = envVarName;
        }

        public bool ShouldUpdateSnap()
        {
            var env = Environment.GetEnvironmentVariable(_envVarName);
            if (env == null)
                return false;
            return bool.TryParse(env, out var value) && value;
        }
    }
}