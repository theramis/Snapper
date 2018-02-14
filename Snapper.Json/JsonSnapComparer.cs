using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json
{
    public class JsonSnapComparer : ISnapComparer
    {
        public bool Compare(object oldSnap, object newSnap)
        {
            var old = JToken.FromObject(oldSnap);

            var @new = JToken.FromObject(newSnap);
            @new = JObject.Parse(@new.ToString());
            return JToken.DeepEquals(old, @new);
        }
    }
}
