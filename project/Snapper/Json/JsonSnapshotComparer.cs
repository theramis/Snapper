using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json
{
    internal class JsonSnapshotComparer : ISnapshotComparer
    {
        public bool CompareSnapshots(object oldSnap, object newSnap)
        {
            var old = JObjectHelper.FromObject(oldSnap);

            var @new = JObjectHelper.FromObject(newSnap);
            @new = JObjectHelper.ParseFromString(@new.ToString());
            return JToken.DeepEquals(old, @new);
        }
    }
}
