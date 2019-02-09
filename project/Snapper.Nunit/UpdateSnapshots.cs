using System;

namespace Snapper.Nunit
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class UpdateSnapshots : Attribute
    {
    }
}
