using System;
using Newtonsoft.Json;

namespace Snapper
{
    public static class SnapperSettings
    {
        public static Func<JsonSerializerSettings> SnapshotDeserializationSettings { get; set; } = () => null;
    }
}
