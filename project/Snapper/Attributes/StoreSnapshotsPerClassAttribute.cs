using System;

namespace Snapper.Attributes
{
    // TODO add docs here
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
    public class StoreSnapshotsPerClassAttribute : Attribute
    {
    }
}
