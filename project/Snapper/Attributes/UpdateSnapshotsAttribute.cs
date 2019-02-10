using System;

namespace Snapper.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
    public class UpdateSnapshotsAttribute : Attribute
    {
    }
}
