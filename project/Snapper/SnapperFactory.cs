using System;
using Snapper.Core;
using Snapper.Core.TestMethodResolver;
using Snapper.Json;

namespace Snapper
{
    internal static class SnapperFactory
    {
        public static Snapper GetJsonSnapper() => JsonSnapper.Value;
        private static readonly Lazy<Snapper> JsonSnapper = new(CreateJsonSnapper);

        private static Snapper CreateJsonSnapper()
        {
            var testMethodResolver = new TestMethodResolver();
            return new Snapper(new JsonSnapshotStore(), new SnapshotUpdateDecider(testMethodResolver),
                new JsonSnapshotComparer(), new SnapshotIdResolver(testMethodResolver), new JsonSnapshotSanitiser(),
                new SnapshotAsserter());
        }

        public static Snapper GetJsonInlineSnapper(object expectedSnapshot)
        {
            var testMethodResolver = new TestMethodResolver();
            var jsonSnapshotSanitiser = new JsonSnapshotSanitiser();
            return new Snapper(new JsonSnapshotInMemoryStore(jsonSnapshotSanitiser, expectedSnapshot),
                new AlwaysFalseSnapshotUpdateDecider(), new JsonSnapshotComparer(),
                new SnapshotIdResolver(testMethodResolver), jsonSnapshotSanitiser,
                new SnapshotAsserter());
        }
    }
}
