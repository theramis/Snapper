using System;

namespace Snapper.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
public class StoreSnapshotsPerClassAttribute : Attribute
{
}
