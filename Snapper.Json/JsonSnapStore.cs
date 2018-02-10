using System.IO;
using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json
{
    public class JsonSnapStore : ISnapStore
    {
        public object GetSnap(string path)
            => File.Exists(path) ? JObject.Parse(File.ReadAllText(path)) : null;

        public void StoreSnap(string path, object value)
        {
            var snap = JToken.FromObject(value);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, snap.ToString());
        }
    }
}