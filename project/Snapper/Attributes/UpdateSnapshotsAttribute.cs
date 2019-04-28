using System;

namespace Snapper.Attributes
{
    /// <inheritdoc />
    /// <summary>
    ///     Tells Snapper to update snapshots. Can be placed on methods, classes and assembly.
    ///     Note: Has no effect when on inline snapshots
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
    public class UpdateSnapshotsAttribute : Attribute
    {
    }
}
