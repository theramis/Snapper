using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json
{
    public class JsonSnapStore : ISnapStore
    {
        private readonly IFileSystem _fileSystem;

        public JsonSnapStore(IFileSystem fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
        }
        
        public object GetSnap(string path)
            => _fileSystem.FileExists(path) ? JObject.Parse(_fileSystem.ReadTextFromFile(path)) : null;

        public void StoreSnap(string path, object value)
        {
            var snap = JToken.FromObject(value);
            _fileSystem.CreateFolder(_fileSystem.GetFolderPath(path));
            _fileSystem.WriteTextToFile(path, snap.ToString());
        }
    }
}
