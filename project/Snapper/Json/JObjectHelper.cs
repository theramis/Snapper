using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Snapper.Json
{
    public static class JObjectHelper
    {
        private static readonly JsonSerializerSettings JsonSettings;

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

        static JObjectHelper()
        {
            var type = typeof(ICustomSnapshotSerializerSettings);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && p.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            if (types.Any())
            {
                var customSettings = types.Count == 1
                    ? types.First()
                    : throw new Exception(
                        $"Found more than on class implementing custom serializer settings '{string.Join(", ", types.Select(t => t.FullName))}', please make sure there is only one");
                var customSerializerSettings = (ICustomSnapshotSerializerSettings) Activator.CreateInstance(customSettings);
                JsonSettings = customSerializerSettings.JsonSerializerSettings;
            }
            else
            {
                JsonSettings = Default;
            }
        }

        private static JsonSerializerSettings Default => new() {DateParseHandling = DateParseHandling.None, MetadataPropertyHandling = MetadataPropertyHandling.Ignore};
    }

   
    public interface ICustomSnapshotSerializerSettings
    {
        JsonSerializerSettings JsonSerializerSettings { get; }
    }
}
