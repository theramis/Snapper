using System;
using System.Linq;
using Newtonsoft.Json;

namespace Snapper
{
    public abstract class SnapperSettings
    {
        private static readonly JsonSerializerSettings Default = new JsonSerializerSettings
        {
            DateParseHandling = DateParseHandling.None, 
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore
        };
        
        public static readonly JsonSerializerSettings JsonSettings = Default;
       

        static SnapperSettings()
        {
            var type = typeof(SnapperSettings);

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && p.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            if (!types.Any())
            {
                return;
            }

            var customSettings = types.Count == 1
                ? types.First()
                : throw new Exception(
                    $"Found more than on class implementing custom settings '{string.Join(", ", types.Select(t => t.FullName))}', please make sure there is only one");
            var customSerializerSettings = (SnapperSettings) Activator.CreateInstance(customSettings);
            JsonSettings = customSerializerSettings.Customize(Default);
        }

        protected virtual JsonSerializerSettings Customize(JsonSerializerSettings defaultSettings)
        {
            return Default;
        }
    }
}
