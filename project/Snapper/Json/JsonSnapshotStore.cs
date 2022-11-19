using System.IO;
using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json;

internal class JsonSnapshotStore : ISnapshotStore
{
    private readonly SnapshotSettings _snapshotSettings;

    public JsonSnapshotStore(SnapshotSettings snapshotSettings)
    {
        _snapshotSettings = snapshotSettings;
    }

    public JsonSnapshot? GetSnapshot(SnapshotId snapshotId)
    {
        if (!File.Exists(snapshotId.FilePath))
            return null;

        var fullSnapshot = JObjectHelper.ParseFromString(File.ReadAllText(snapshotId.FilePath),
            _snapshotSettings);

        if (snapshotId.PrimaryId == null && snapshotId.SecondaryId == null)
            return new JsonSnapshot(snapshotId, fullSnapshot);

        if (snapshotId.PrimaryId != null &&
            fullSnapshot.TryGetValue(snapshotId.PrimaryId, out var partialSnapshot))
        {
            if (snapshotId.SecondaryId != null &&
                partialSnapshot is JObject partialSnapshotJObject &&
                partialSnapshotJObject.TryGetValue(snapshotId.SecondaryId, out partialSnapshot))
            {
                return new JsonSnapshot(snapshotId, (JObject) partialSnapshot);
            }

            return new JsonSnapshot(snapshotId, (JObject) partialSnapshot);
        }
        return null;
    }

    public void StoreSnapshot(JsonSnapshot newSnapshot)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(newSnapshot.Id.FilePath));

        JToken newSnapshotToWrite;

        if (newSnapshot.Id.PrimaryId == null && newSnapshot.Id.SecondaryId == null)
        {
            newSnapshotToWrite = newSnapshot.Value;
        }
        else
        {
            var rawSnapshotContent = GetRawSnapshotContent(newSnapshot.Id.FilePath);
            newSnapshotToWrite = rawSnapshotContent == null
                ? new JObject()
                : JObjectHelper.ParseFromString(rawSnapshotContent, _snapshotSettings);

            if (newSnapshot.Id.PrimaryId != null && newSnapshot.Id.SecondaryId == null)
            {
                newSnapshotToWrite[newSnapshot.Id.PrimaryId] = newSnapshot.Value;
            }
            else if (newSnapshot.Id.PrimaryId != null && newSnapshot.Id.SecondaryId != null)
            {
                var firstLevel = newSnapshotToWrite[newSnapshot.Id.PrimaryId];
                if (firstLevel == null)
                    newSnapshotToWrite[newSnapshot.Id.PrimaryId] = new JObject();

                newSnapshotToWrite[newSnapshot.Id.PrimaryId][newSnapshot.Id.SecondaryId] = newSnapshot.Value;
            }
        }

        File.WriteAllText(newSnapshot.Id.FilePath, newSnapshotToWrite.ToString());
    }

    private static string? GetRawSnapshotContent(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        var content = File.ReadAllText(filePath);
        return string.IsNullOrWhiteSpace(content) ? null : content;
    }
}
