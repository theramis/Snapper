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

            var (method, _) = XUnitTestHelper.GetCallingTestInfo();

            return method?.GetCustomAttributes(typeof(UpdateTestSnapshots), true).Any() ?? false;
        }
    }
}
