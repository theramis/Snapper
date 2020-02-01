using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Snapper.Json
{
    internal static class JObjectHelper
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            DateParseHandling = DateParseHandling.None,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore
        };

        public static JObject ParseFromString(string jsonString)
        {
            return JsonConvert.DeserializeObject(jsonString, JsonSettings) as JObject;
        }

        public static JObject FromObject(object obj)
        {
            if (obj is JObject result)
                return result;

            return JObject.FromObject(obj, JsonSerializer.Create(JsonSettings));
        }
    }
}
