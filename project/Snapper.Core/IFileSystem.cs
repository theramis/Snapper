using System.IO;

namespace Snapper.Core
{
    public interface IFileSystem
    {
        bool FileExists(string filePath);

        string ReadTextFromFile(string filePath);

        void WriteTextToFile(string filePath, string text);

        void CreateFolder(string folderPath);

        string GetFolderPath(string filePath);
    }

    public class FileSystem : IFileSystem
    {
        public bool FileExists(string filePath)
            => File.Exists(filePath);

        public string ReadTextFromFile(string filePath)
            => File.ReadAllText(filePath);

        public void WriteTextToFile(string filePath, string text)
            => File.WriteAllText(filePath, text);

        public void CreateFolder(string folderPath)
            => Directory.CreateDirectory(folderPath);

        public string GetFolderPath(string filePath)
            => Path.GetDirectoryName(filePath);
    }
}
