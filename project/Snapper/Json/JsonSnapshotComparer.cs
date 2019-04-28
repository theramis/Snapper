using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json
{
    // TODO write tests for this class
    internal class JsonSnapshotComparer : ISnapshotComparer
    {
        public bool CompareSnapshots(object oldSnap, object newSnap)
        {
            var old = JObject.FromObject(oldSnap);

            var @new = JObject.FromObject(newSnap);
            @new = JObject.Parse(@new.ToString());
            return JToken.DeepEquals(old, @new);
        }
    }
}
