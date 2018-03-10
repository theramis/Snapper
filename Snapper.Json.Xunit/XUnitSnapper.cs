using Snapper.Core;

namespace Snapper.Json.Xunit
{
    public class XUnitSnapper : JsonSnapper
    {
        internal static XUnitSnapper Create()
            => new XUnitSnapper(new XUnitAsserter(), new JsonSnapStore(), new XUnitPathResolver(), new XUnitEnvironmentVariableUpdateDecider(), new JsonSnapComparer());

        private XUnitSnapper(IAssert asserter, ISnapStore store, IPathResolver resolver, ISnapUpdateDecider snapUpdateDecider, ISnapComparer comparer)
            : base(asserter, store, resolver, snapUpdateDecider, comparer)
        {
        }

        internal void SnapObject(object value)
            => Snap(string.Empty, value);
    }
}
