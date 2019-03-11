using System;
using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json
{
    // TODO write tests for this class
    internal class JsonSnapshotSanitiser
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
