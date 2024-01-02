using System.Text.Json;
using System.Text.Json.Nodes;

namespace Snapper.Json;

internal class JsonSnapshotSanitiser
{
    private readonly SnapshotSettings _snapshotSettings;

    public JsonSnapshotSanitiser(SnapshotSettings snapshotSettings)
    {
        _snapshotSettings = snapshotSettings;
    }

    public JsonNode SanitiseSnapshot(object snapshot)
    {
        if (snapshot is string stringSnapshot)
        {
            try
            {
                return JsonNodeHelper.ParseFromString(stringSnapshot, _snapshotSettings);
            }
            catch (JsonException e)
            {
            }
        }

        return JsonNodeHelper.FromObject(snapshot, _snapshotSettings);
    }
}
