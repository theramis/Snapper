using Snapper.Core;
using Snapper.Json;

namespace Snapper;

internal class Snapper
{
    private readonly JsonSnapshotGenerator _jsonSnapshotGenerator;
    private readonly SnapperCore _snapperCore;

    public Snapper(JsonSnapshotGenerator jsonSnapshotGenerator, SnapperCore snapperCore)
    {
        _jsonSnapshotGenerator = jsonSnapshotGenerator;
        _snapperCore = snapperCore;
    }

    public SnapResult MatchSnapshot(object rawSnapshot, string? childSnapshotName)
    {
        var snapshot = _jsonSnapshotGenerator.Generate(rawSnapshot, childSnapshotName);
        return _snapperCore.Snap(snapshot);
    }
}
