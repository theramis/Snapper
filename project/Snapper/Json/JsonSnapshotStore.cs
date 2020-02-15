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

            var fullSnapshot = JObjectHelper.ParseFromString(File.ReadAllText(snapshotId.FilePath));

            if (snapshotId.PrimaryId == null && snapshotId.SecondaryId == null)
                return fullSnapshot;

            if (snapshotId.PrimaryId != null &&
                fullSnapshot.TryGetValue(snapshotId.PrimaryId, out var partialSnapshot))
            {
                if (snapshotId.SecondaryId != null &&
                    partialSnapshot is JObject partialSnapshotJObject &&
                    partialSnapshotJObject.TryGetValue(snapshotId.SecondaryId, out partialSnapshot))
                {
                    return partialSnapshot;
                }

                return partialSnapshot;
            }
            return null;
        }

        public void StoreSnapshot(SnapshotId snapshotId, object value)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(snapshotId.FilePath));

            var newSnapshot = JObjectHelper.FromObject(value);

            JToken newSnapshotToWrite;

            if (snapshotId.PrimaryId == null && snapshotId.SecondaryId == null)
            {
                newSnapshotToWrite = newSnapshot;
            }
            else
            {
                var rawSnapshotContent = GetRawSnapshotContent(snapshotId.FilePath);
                newSnapshotToWrite = rawSnapshotContent == null
                    ? new JObject()
                    : JObject.Parse(rawSnapshotContent);

                if (snapshotId.PrimaryId != null && snapshotId.SecondaryId == null)
                {
                    newSnapshotToWrite[snapshotId.PrimaryId] = newSnapshot;
                }
                else if (snapshotId.PrimaryId != null && snapshotId.SecondaryId != null)
                {
                    var firstLevel = newSnapshotToWrite[snapshotId.PrimaryId];
                    if (firstLevel == null)
                        newSnapshotToWrite[snapshotId.PrimaryId] = new JObject();

                    newSnapshotToWrite[snapshotId.PrimaryId][snapshotId.SecondaryId] = newSnapshot;
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
