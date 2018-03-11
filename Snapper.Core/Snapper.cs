namespace Snapper.Core
{
    public class Snapper : SnapperCore
    {
        #if NETSTANDARD2_0
        public Snapper(IAssert asserter, string snapshotDirectory)
            : base(asserter, new ByteSnapStore(), new StaticPathResolver(snapshotDirectory), new EnvironmentVariableUpdateDecider(),
                new DefaultSnapComparer())
        {}
        #endif

        public Snapper(IAssert asserter, ISnapStore snapStore, string snapshotDirectory)
            : base(asserter, snapStore, new StaticPathResolver(snapshotDirectory), new EnvironmentVariableUpdateDecider(),
                new DefaultSnapComparer())
        {}
    }
}
