using Snapper.Core;
using Snapper.Core.TestMethodResolver;
using Snapper.Json;

namespace Snapper;

internal static class SnapperFactory
{
    public static Snapper CreateJsonSnapper(SnapshotSettings? settings)
    {
        settings ??= SnapshotSettings.New();
        var testMethodResolver = new TestMethodResolver();
        return new Snapper(new JsonSnapshotStore(settings), new SnapshotUpdateDecider(testMethodResolver),
            new SnapshotIdResolver(testMethodResolver), new JsonSnapshotSanitiser(settings), settings);
    }

    public static Snapper GetJsonInlineSnapper(object expectedSnapshot, SnapshotSettings? settings)
    {
        settings ??= SnapshotSettings.New();
        // TODO can probably remove the test resolver
        var testMethodResolver = new TestMethodResolver();
        var jsonSnapshotSanitiser = new JsonSnapshotSanitiser(settings);
        return new Snapper(new JsonSnapshotInMemoryStore(jsonSnapshotSanitiser, settings, expectedSnapshot),
            new AlwaysFalseSnapshotUpdateDecider(), new SnapshotIdResolver(testMethodResolver),
            jsonSnapshotSanitiser, settings);
    }
}
