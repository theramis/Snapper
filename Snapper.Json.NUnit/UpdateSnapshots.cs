using System;

namespace Snapper.Json.NUnit
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class UpdateSnapshots : Attribute
    {
    }
}
