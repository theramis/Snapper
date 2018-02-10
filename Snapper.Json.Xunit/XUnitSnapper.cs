using Snapper.Core;

namespace Snapper.Json.Xunit
{
    public class XUnitSnapper : JsonSnapper
    {
        public static void Snap(object value)
            => Create().SnapObject(value);

        private static XUnitSnapper Create()
            => new XUnitSnapper(new XUnitAsserter(), new JsonSnapStore(), new XUnitPathResolver(), new EnvironmentVariableUpdateDecider(), new JsonSnapComparer());

        protected XUnitSnapper(IAssert asserter, ISnapStore store, IPathResolver resolver, ISnapUpdateDecider snapUpdateDecider, ISnapComparer comparer) 
            : base(asserter, store, resolver, snapUpdateDecider, comparer)
        {
        }

        private void SnapObject(object value)
            => Snap(string.Empty, value);
    }
}