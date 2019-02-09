using System;

namespace Snapper.Xunit
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class UpdateSnapshots : Attribute
    {
    }
}
