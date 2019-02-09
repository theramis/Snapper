using System.IO;
using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json
{
    public class JsonSnapshotStore : ISnapshotStore
    {
        public object GetSnapshot(string path)
            => File.Exists(path) ? JObject.Parse(File.ReadAllText(path)) : null;

        public void StoreSnapshot(string path, object value)
        {
            var snap = JToken.FromObject(value);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, snap.ToString());
        }
    }
}
