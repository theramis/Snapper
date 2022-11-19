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
        var jsonSnapshotGenerator = new JsonSnapshotGenerator(new SnapshotIdResolver(testMethodResolver),
            new JsonSnapshotSanitiser(settings), settings);
        var snapperCore = new SnapperCore(new JsonSnapshotStore(settings),
                new SnapshotUpdateDecider(testMethodResolver));
        return new Snapper(jsonSnapshotGenerator, snapperCore);
    }

    public static Snapper GetJsonInlineSnapper(object expectedSnapshot, SnapshotSettings? settings)
    {
        settings ??= SnapshotSettings.New();

        var jsonSnapshotSanitiser = new JsonSnapshotSanitiser(settings);

        var jsonSnapshotGenerator = new JsonSnapshotGenerator(new EmptySnapshotIdResolver(), jsonSnapshotSanitiser,
            settings);
        var snapperCore = new SnapperCore(new JsonSnapshotInMemoryStore(jsonSnapshotSanitiser, expectedSnapshot),
            new AlwaysFalseSnapshotUpdateDecider());
        return new Snapper(jsonSnapshotGenerator, snapperCore);
    }
}
