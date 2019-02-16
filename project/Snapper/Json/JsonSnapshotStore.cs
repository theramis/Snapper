using System.IO;
using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json
{
    // TODO write tests for this class
    internal class JsonSnapshotStore : ISnapshotStore
    {
        public object GetSnapshot(SnapshotId snapshotId)
        {
            if (!File.Exists(snapshotId.FilePath))
                return null;

            var fullSnapshot = JObject.Parse(File.ReadAllText(snapshotId.FilePath));

            if (string.IsNullOrWhiteSpace(snapshotId.PartialId))
                return fullSnapshot;

            return fullSnapshot.TryGetValue(snapshotId.PartialId, out var partialSnapshot)
                ? partialSnapshot
                : null;
        }

        public void StoreSnapshot(SnapshotId snapshotId, object value)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(snapshotId.FilePath));

            var newSnapshot = JToken.FromObject(value);

            JToken newSnapshotToWrite;

            if (snapshotId.PartialId == null)
            {
                newSnapshotToWrite = newSnapshot;
            }
            else
            {
                var rawSnapshotContent = GetRawSnapshotContent(snapshotId.FilePath);
                if (rawSnapshotContent == null)
                {
                    newSnapshotToWrite = new JObject
                    {
                        [snapshotId.PartialId] = newSnapshot
                    };
                }
                else
                {
                    newSnapshotToWrite = JObject.Parse(rawSnapshotContent);
                    newSnapshotToWrite[snapshotId.PartialId] = newSnapshot;
                }
            }

            File.WriteAllText(snapshotId.FilePath, newSnapshotToWrite.ToString());
        }

        private static string GetRawSnapshotContent(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            var content = File.ReadAllText(filePath);
            return string.IsNullOrWhiteSpace(content) ? null : content;
        }
    }
}
