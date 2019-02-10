using System;
using Snapper.Core;
using Snapper.Json;

namespace Snapper
{
    internal static class SnapperFactory
    {
        public static Snapper GetJsonSnapper() => JsonSnapper.Value;

        private static readonly Lazy<Snapper> JsonSnapper =
            new Lazy<Snapper>(CreateJsonSnapper);

        private static Snapper CreateJsonSnapper()
            => new Snapper(new JsonSnapshotStore(), new SnapshotUpdateDecider(),
                new JsonSnapshotComparer(), new SnapshotIdResolver(), new JsonSnapshotSanitiser(),
                new SnapshotAsserter());
    }
}
