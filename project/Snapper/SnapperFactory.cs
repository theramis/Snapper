using Snapper.Core;
using Snapper.Json;

namespace Snapper
{
    internal static class SnapperFactory
    {
        // TODO Implement properly
        public static Snapper CreateJsonSnapper()
            => new Snapper(new JsonSnapshotStore(), new EnvironmentVariableUpdateDecider(),
                new JsonSnapshotComparer(), new SnapshotIdResolver());
    }
}
