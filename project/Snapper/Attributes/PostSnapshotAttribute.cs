using System;
using System.Collections.Generic;
using System.Text;
using Snapper.Core;

namespace Snapper.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
    public abstract class PostSnapshotAttribute : Attribute
    {
        public abstract void Handle(SnapResult result);
    }
}
