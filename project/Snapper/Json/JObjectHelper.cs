using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Snapper.Json
{
    public static class JObjectHelper
    {
        public static JObject ParseFromString(string jsonString) => JsonConvert.DeserializeObject(jsonString, SnapperSettings.JsonSettings) as JObject;

        public static JObject FromObject(object obj)
        {
            if (obj is JObject result)
            {
                return result;
            }

            return JObject.FromObject(obj, JsonSerializer.Create(SnapperSettings.JsonSettings));
        }
    }
}
