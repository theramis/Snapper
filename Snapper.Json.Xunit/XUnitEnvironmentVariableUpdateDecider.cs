using System.Linq;
using Snapper.Core;

namespace Snapper.Json.Xunit
{
    public class XUnitEnvironmentVariableUpdateDecider : ISnapUpdateDecider
    {
        private readonly ISnapUpdateDecider _envUpdateDecider;

        public XUnitEnvironmentVariableUpdateDecider()
        {
            _envUpdateDecider = new EnvironmentVariableUpdateDecider();
        }

        public bool ShouldUpdateSnap()
        {
            if (_envUpdateDecider.ShouldUpdateSnap())
                return true;

            var info = XUnitTestHelper.GetCallingTestInfo();

            var methodHasAttribute = info.Method?.GetCustomAttributes(typeof(UpdateSnapshots), true).Any() ?? false;
            var classHasAttribute =
                info.Method?.ReflectedType?.GetCustomAttributes(typeof(UpdateSnapshots), true).Any() ?? false;

            return methodHasAttribute || classHasAttribute;
        }
    }
}
