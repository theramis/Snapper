using System;
using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json
{
    internal class JsonSnapshotSanitiser : ISnapshotSanitiser
    {
        public object SanitiseSnapshot(object snapshot)
        {
            try
            {
                JObject.FromObject(snapshot);
                return snapshot;
            }
            catch (ArgumentException)
            {
                return new { snapshot };
            }
        }
    }
}
