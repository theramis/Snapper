using System;
using Snapper.Core;
using Snapper.Core.TestMethodResolver;
using Snapper.Json;

namespace Snapper
{
    internal static class SnapperFactory
    {
        public static Snapper GetJsonSnapper() => JsonSnapper.Value;
        private static readonly Lazy<Snapper> JsonSnapper =
            new Lazy<Snapper>(CreateJsonSnapper);

        private static Snapper CreateJsonSnapper()
        {
            var testMethodResolver = new TestMethodResolver();

            var snapshotHandler = new CoreSnapshotHandler(
                new JsonSnapshotStore(),
                new JsonSnapshotComparer(),
                new SnapshotUpdateDecider(testMethodResolver));

            return new Snapper(
                new SnapshotIdResolver(testMethodResolver),
                new JsonSnapshotSanitiser(),
                new SnapshotAsserter(),
                new PostSnapshotHandler(snapshotHandler, testMethodResolver));
        }

        public static Snapper GetJsonInlineSnapper(object expectedSnapshot)
        {
            var testMethodResolver = new TestMethodResolver();

            var jsonSnapshotSanitiser = new JsonSnapshotSanitiser();

            var snapshotHandler = new CoreSnapshotHandler(
                new JsonSnapshotInMemoryStore(jsonSnapshotSanitiser, expectedSnapshot),
                new JsonSnapshotComparer(),
                new AlwaysFalseSnapshotUpdateDecider());

            return new Snapper(
                new SnapshotIdResolver(testMethodResolver),
                jsonSnapshotSanitiser,
                new SnapshotAsserter(),
                snapshotHandler);
        }
    }
}
