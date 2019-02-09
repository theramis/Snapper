using System.Linq;
using Snapper.Core;

namespace Snapper.Xunit
{
    public class XUnitEnvironmentVariableUpdateDecider : ISnapshotUpdateDecider
    {
        private readonly ISnapshotUpdateDecider _envUpdateDecider;

        public XUnitEnvironmentVariableUpdateDecider()
        {
            _envUpdateDecider = new EnvironmentVariableUpdateDecider();
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
