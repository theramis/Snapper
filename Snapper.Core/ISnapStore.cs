using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Snapper.Core
{
    public interface ISnapStore
    {
        object GetSnap(string path);

        void StoreSnap(string path, object value);
    }

    public class ByteSnapStore : ISnapStore
    {
        private readonly IFileSystem _fileSystem;

        public ByteSnapStore(IFileSystem fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
        }

        public object GetSnap(string path)
            => _fileSystem.FileExists(path) ? StringToObject(_fileSystem.ReadTextFromFile(path)) : null;

        public void StoreSnap(string path, object value)
        {
            _fileSystem.CreateFolder(_fileSystem.GetFolderPath(path));
            _fileSystem.WriteTextToFile(path, ObjectToString(value));
        }

        private static object StringToObject(string data)
        {
            var bytes = Convert.FromBase64String(data);
            var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }

        private static string ObjectToString(object obj)
        {
            if (obj == null)
                return null;
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            var bytes = ms.ToArray();
            return Convert.ToBase64String(bytes);
        }
    }
}
