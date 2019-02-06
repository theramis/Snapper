using Snapper.Core;

namespace Snapper.Json
{
    public class JsonSnapper : SnapperCore
    {
        public JsonSnapper(IAssert asserter, string snapshotDirectory)
            : base(asserter, new JsonSnapStore(), new StaticPathResolver(snapshotDirectory), new EnvironmentVariableUpdateDecider(),
                new JsonSnapComparer())
        {}

        protected JsonSnapper(IAssert asserter, ISnapStore store, IPathResolver resolver, ISnapUpdateDecider snapUpdateDecider, ISnapComparer comparer)
            : base(asserter, store, resolver, snapUpdateDecider, comparer)
        {}
    }
}