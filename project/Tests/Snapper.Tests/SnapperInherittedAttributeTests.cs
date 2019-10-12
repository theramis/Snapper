using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace Snapper.Tests
{
    public class SnapperInherittedAttributeTests
    {
        [NewAndImprovedFactAttribute]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void SnapshotsMatch_UsingInherittedAttribute()
        {
            var snapshot = new
            {
                TestValue = "value"
            };

            snapshot.ShouldMatchInlineSnapshot(new
            {
                TestValue = "value"
            });
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class NewAndImprovedFactAttribute : FactAttribute
    {
        public NewAndImprovedFactAttribute(params Type[] skippingExceptions)
        {
        }
    }
}
