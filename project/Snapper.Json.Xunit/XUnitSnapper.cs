using Snapper.Core;

namespace Snapper.Json.Xunit
{
    public class XUnitSnapper : JsonSnapper
    {
        private static XUnitSnapper Create()
            => new XUnitSnapper(new XUnitAsserter(), new JsonSnapStore(), new XUnitPathResolver(), new XUnitEnvironmentVariableUpdateDecider(), new JsonSnapComparer());

        private XUnitSnapper(IAssert asserter, ISnapStore store, IPathResolver resolver, ISnapUpdateDecider snapUpdateDecider, ISnapComparer comparer)
            : base(asserter, store, resolver, snapUpdateDecider, comparer)
        {
        }

        private void SnapObject(object value)
            => Snap(string.Empty, value);

        public static void MatchSnapshot(object value)
        {
            Create().SnapObject(value);
        }

        public static void MatchSnapshot(string snapshotName, object value)
        {
            Create().Snap(snapshotName, value);
        }
    }
}
