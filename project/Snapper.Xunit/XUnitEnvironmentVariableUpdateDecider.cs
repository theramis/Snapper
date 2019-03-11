using System.Linq;
using Snapper.Core;
using Snapper.Core.TestMethodResolver;

namespace Snapper.Xunit
{
    public class XUnitEnvironmentVariableUpdateDecider : ISnapshotUpdateDecider
    {
        private readonly ISnapshotUpdateDecider _envUpdateDecider;

        public XUnitEnvironmentVariableUpdateDecider()
        {
            _envUpdateDecider = new SnapshotUpdateDecider(new TestMethodResolver());
        }

        public bool ShouldUpdateSnapshot()
        {
            if (_envUpdateDecider.ShouldUpdateSnapshot())
                return true;

            var (method, _) = XUnitTestHelper.GetCallingTestInfo();

            var methodHasAttribute = method?.GetCustomAttributes(typeof(UpdateSnapshots), true).Any() ?? false;
            var classHasAttribute =
                method?.ReflectedType?.GetCustomAttributes(typeof(UpdateSnapshots), true).Any() ?? false;

            return methodHasAttribute || classHasAttribute;
        }
    }
}
