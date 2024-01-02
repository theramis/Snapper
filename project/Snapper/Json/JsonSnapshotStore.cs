using System.IO;
using System.Text.Json.Nodes;
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

        var fullSnapshot = JsonNodeHelper.ParseFromString(File.ReadAllText(snapshotId.FilePath),
            _snapshotSettings);

        if (snapshotId.PrimaryId == null && snapshotId.SecondaryId == null)
            return new JsonSnapshot(snapshotId, fullSnapshot);

        if (snapshotId.PrimaryId != null &&
            fullSnapshot.TryGetValue(snapshotId.PrimaryId, out var partialSnapshot))
        {
            if (snapshotId.SecondaryId != null)
            {
                if (partialSnapshot.TryGetValue(snapshotId.SecondaryId, out partialSnapshot))
                {
                    return new JsonSnapshot(snapshotId, partialSnapshot);
                }

                return null;
            }
            return new JsonSnapshot(snapshotId, partialSnapshot);
        }
        return null;
    }

    public void StoreSnapshot(JsonSnapshot newSnapshot)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(newSnapshot.Id.FilePath));

        if (newSnapshot.Id.PrimaryId == null && newSnapshot.Id.SecondaryId == null)
        {
            var newSnapshotToWrite = newSnapshot.Value;
            File.WriteAllText(newSnapshot.Id.FilePath, JsonNodeHelper.ToString(newSnapshotToWrite, _snapshotSettings));
        }
        else
        {
            var rawSnapshotContent = GetRawSnapshotContent(newSnapshot.Id.FilePath);
            var newSnapshotToWrite = rawSnapshotContent == null
                ? new JsonObject()
                : JsonNodeHelper.ParseFromString(rawSnapshotContent, _snapshotSettings)!;

            if (newSnapshot.Id.PrimaryId != null && newSnapshot.Id.SecondaryId == null)
            {
                newSnapshotToWrite[newSnapshot.Id.PrimaryId] = newSnapshot.Value;
            }
            else if (newSnapshot.Id.PrimaryId != null && newSnapshot.Id.SecondaryId != null)
            {
                var firstLevel = newSnapshotToWrite[newSnapshot.Id.PrimaryId];
                if (firstLevel == null)
                    newSnapshotToWrite[newSnapshot.Id.PrimaryId] = new JsonObject();

                newSnapshotToWrite[newSnapshot.Id.PrimaryId]![newSnapshot.Id.SecondaryId] = newSnapshot.Value;
            }
            File.WriteAllText(newSnapshot.Id.FilePath, JsonNodeHelper.ToString(newSnapshotToWrite, _snapshotSettings));
        }
    }

    private static string? GetRawSnapshotContent(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        var content = File.ReadAllText(filePath);
        return string.IsNullOrWhiteSpace(content) ? null : content;
    }
}
