using System.Collections.Generic;
using Snapper.Attributes;
using Snapper.Core.TestMethodResolver;

namespace Snapper.Core
{
    internal class PostSnapshotHandler : ISnapshotHandler
    {
        private readonly ISnapshotHandler _snapshotHandler;
        private readonly ITestMethodResolver _testMethodResolver;

        public PostSnapshotHandler(
            ISnapshotHandler snapshotHandler,
            ITestMethodResolver testMethodResolver)
        {
            _snapshotHandler = snapshotHandler;
            _testMethodResolver = testMethodResolver;
        }

        public SnapResult Snap(SnapshotId id, object newSnapshot)
        {
            var method = _testMethodResolver.ResolveTestMethod().BaseMethod;

            var result = _snapshotHandler.Snap(id, newSnapshot);

            foreach (var attr in method.GetCustomAttributes<PostSnapshotAttribute>())
            {
                attr.Handle(result);
            }

            return result;
        }
    }
}
