using Snapper.Core;

namespace Snapper.Json.Nunit
{
    internal class NUnitSnapper : JsonSnapper
    {
        private static NUnitSnapper Create()
            => new NUnitSnapper(new NUnitAsserter(), new JsonSnapStore(), new NUnitPathResolver(),
                new NUnitEnvironmentVariableUpdateDecider(), new JsonSnapComparer());

        private NUnitSnapper(IAssert asserter, ISnapStore store, IPathResolver resolver,
            ISnapUpdateDecider snapUpdateDecider, ISnapComparer comparer)
            : base(asserter, store, resolver, snapUpdateDecider, comparer)
        {
        }

        private void SnapObject(object value)
            => Snap(string.Empty, value);

        public static SnapResults MatchSnapshot(object value)
        {
            try
            {
                Create().SnapObject(value);
            }
            catch (NUnitAsserterException e)
            {
                return e.Results;
            }

            return null;
        }

        public static SnapResults MatchSnapshot(string snapshotName, object value)
        {
            try
            {
                Create().Snap(snapshotName, value);
            }
            catch (NUnitAsserterException e)
            {
                return e.Results;
            }

            return null;
        }
    }
}
