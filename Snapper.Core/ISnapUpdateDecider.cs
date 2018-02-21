using System;
using System.IO;

namespace Snapper.Core
{
    public interface ISnapUpdateDecider
    {
        bool ShouldUpdateSnap();
    }

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

    public sealed class UpdateFileUpdateDecider : ISnapUpdateDecider
    {
        private readonly string _updateFilePath;

        public UpdateFileUpdateDecider(string updateFilePath)
        {
            _updateFilePath = updateFilePath;
        }

        public bool ShouldUpdateSnap()
            => File.Exists(_updateFilePath);
    }
}
