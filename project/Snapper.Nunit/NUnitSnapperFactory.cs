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
            return new NUnitSnapper(new JsonSnapshotStore(), new SnapshotUpdateDecider(testMethodResolver),
                new JsonSnapshotComparer(), new SnapshotIdResolver(testMethodResolver), new JsonSnapshotSanitiser());
        }
    }
}
