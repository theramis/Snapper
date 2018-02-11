namespace Snapper.Core
{
    public class Snapper : SnapperCore
    {
        public Snapper(IAssert asserter, string snapshotDirectory)
            : base(asserter, new ByteSnapStore(), new StaticPathResolver(snapshotDirectory), new EnvironmentVariableUpdateDecider(),
                new DefaultSnapComparer())
        {
        }

        private Snapper(IAssert asserter, ISnapStore store, IPathResolver resolver, ISnapUpdateDecider snapUpdateDecider, ISnapComparer comparer)
            : base(asserter, store, resolver, snapUpdateDecider, comparer)
        {
        }
    }
}
