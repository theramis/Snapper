using System;

namespace Snapper.Attributes
{
    /// <inheritdoc />
    /// <summary>
    ///     Tells Snapper to update snapshots. Can be placed on methods, classes and assembly.
    ///     Note: Has no effect when using inline snapshots
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
    public class UpdateSnapshotsAttribute : Attribute
    {
        public readonly bool IgnoreIfCi;

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="ignoreIfCi">When set to true the snapshot is not updated when a CI environment is detected.</param>
        public UpdateSnapshotsAttribute(bool ignoreIfCi = true)
        {
            IgnoreIfCi = ignoreIfCi;
        }
    }
}
