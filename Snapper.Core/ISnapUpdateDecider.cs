using System;

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
        private readonly IFileSystem _fileSystem;

        public UpdateFileUpdateDecider(string updateFilePath, IFileSystem fileSystem = null)
        {
            _updateFilePath = updateFilePath;
            _fileSystem = fileSystem ?? new FileSystem();
        }

        public bool ShouldUpdateSnap()
            => _fileSystem.FileExists(_updateFilePath);
    }
}
