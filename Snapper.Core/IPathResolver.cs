using System.IO;

namespace Snapper.Core
{
    public interface IPathResolver
    {
        string ResolvePath(string snapshotName);
    }

    public sealed class StaticPathResolver : IPathResolver
    {
        private readonly string _snapshotFolderPath;

        public StaticPathResolver(string snapshotFolderPath)
        {
            _snapshotFolderPath = snapshotFolderPath;
        }

        public string ResolvePath(string snapshotName)
            => Path.Combine(_snapshotFolderPath, snapshotName);
    }
}