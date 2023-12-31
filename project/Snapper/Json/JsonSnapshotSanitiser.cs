using System;
using System.Text.Json;
using Snapper.Exceptions;

namespace Snapper.Json;

internal class JsonSnapshotSanitiser
{
    private readonly SnapshotSettings _snapshotSettings;

    public JsonSnapshotSanitiser(SnapshotSettings snapshotSettings)
    {
        _snapshotSettings = snapshotSettings;
    }

    public JsonElement SanitiseSnapshot(object snapshot)
    {
        if (snapshot is string stringSnapshot)
        {
            try
            {
                return JsonElementHelper.ParseFromString(stringSnapshot, _snapshotSettings);
            }
            catch (JsonException e)
            {
            }
        }

        return JsonElementHelper.FromObject(snapshot, _snapshotSettings);
    }
}
