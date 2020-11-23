using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json
{
    internal class JsonSnapshotComparer : ISnapshotComparer
    {
        public SnapResult CompareSnapshots(object oldSnap, object newSnap)
        {
            if (oldSnap is null) return SnapResult.SnapshotDoesNotExist(newSnap);
            else if (AreEqual(oldSnap, newSnap)) return SnapResult.SnapshotsMatch(oldSnap, newSnap);
            else return SnapResult.SnapshotsDoNotMatch(oldSnap, newSnap);
        }

        private bool AreEqual(object oldSnapshot, object newSnapshot)
        {
            var old = JObjectHelper.FromObject(oldSnapshot);
            var @new = JObjectHelper.FromObject(newSnapshot);
            @new = JObjectHelper.ParseFromString(@new.ToString());

            return JToken.DeepEquals(old, @new);
        }
    }
}
