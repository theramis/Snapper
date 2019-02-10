using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json
{
    internal class JsonSnapshotComparer : ISnapshotComparer
    {
        public bool CompareSnapshots(object oldSnap, object newSnap)
        {
            var old = JToken.FromObject(oldSnap);

            var @new = JToken.FromObject(newSnap);
            @new = JObject.Parse(@new.ToString());
            return JToken.DeepEquals(old, @new);
        }
    }
}
