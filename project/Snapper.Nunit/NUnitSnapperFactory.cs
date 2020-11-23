using System;
using Snapper.Core;
using Snapper.Core.TestMethodResolver;
using Snapper.Json;

namespace Snapper.Nunit
{
    internal static class NUnitSnapperFactory
    {
        public static NUnitSnapper GetNUnitSnapper() => NUnitSnapper.Value;

        private static readonly Lazy<NUnitSnapper> NUnitSnapper =
            new Lazy<NUnitSnapper>(CreateNUnitSnapper);

        private static NUnitSnapper CreateNUnitSnapper()
        {
            var testMethodResolver = new TestMethodResolver();

            var snapshotHandler = new CoreSnapshotHandler(
                new JsonSnapshotStore(),
                new JsonSnapshotComparer(),
                new SnapshotUpdateDecider(testMethodResolver));

            return new NUnitSnapper(
                new SnapshotIdResolver(testMethodResolver),
                new JsonSnapshotSanitiser(),
                new PostSnapshotHandler(snapshotHandler, testMethodResolver));
        }
    }
}
