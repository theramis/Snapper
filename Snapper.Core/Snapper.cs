namespace Snapper.Core
{
    public class Snapper : SnapperCore
    {
        public static Snapper Create(IAssert asserter, string snapshotDirectory)
            => new Snapper(asserter, new ByteSnapStore(), new StaticPathResolver(snapshotDirectory), new EnvironmentVariableUpdateDecider(),
                new DefaultSnapComparer());

        private Snapper(IAssert asserter, ISnapStore store, IPathResolver resolver, ISnapUpdateDecider snapUpdateDecider, ISnapComparer comparer)
            : base(asserter, store, resolver, snapUpdateDecider, comparer)
        {
        }
    }
}