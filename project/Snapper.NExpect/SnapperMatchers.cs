using NExpect.Interfaces;
using NExpect.MatcherLogic;
using Snapper.Core;

namespace Snapper.NExpect;

public static class SnapperMatchers
{
    private static IMatcherResult MatchSnapshotMatcher<T>(T obj) 
    {
        var snapper = SnapperFactory.GetJsonSnapper();
        var result = snapper.MatchSnapshot(obj);
        
        return new MatcherResult(result.Status is SnapResultStatus.SnapshotsMatch or SnapResultStatus.SnapshotUpdated,
            Messages.GetSnapResultMessage(result));
    }
    
    public static void MatchSnapshot<T>(this ITo<T> to)
        => to.AddMatcher(MatchSnapshotMatcher);
    
    public static void MatchSnapshot<T>(this IToAfterNot<T> to)
        => to.AddMatcher(MatchSnapshotMatcher);
    
    public static void MatchSnapshot<T>(this INotAfterTo<T> to)
        => to.AddMatcher(MatchSnapshotMatcher);
    
    private static IMatcherResult MatchChildSnapshotMatcher<T>(T obj, string childSnapshotName) 
    {
        var snapper = SnapperFactory.GetJsonSnapper();
        var result = snapper.MatchSnapshot(obj, childSnapshotName);
        
        return new MatcherResult(result.Status is SnapResultStatus.SnapshotsMatch or SnapResultStatus.SnapshotUpdated,
            Messages.GetSnapResultMessage(result));
    }
    
    public static void MatchChildSnapshot<T>(this ITo<T> to, string childSnapshotName)
        => to.AddMatcher(obj => MatchChildSnapshotMatcher(obj, childSnapshotName));
    
    public static void MatchChildSnapshot<T>(this IToAfterNot<T> to, string childSnapshotName)
        => to.AddMatcher(obj => MatchChildSnapshotMatcher(obj, childSnapshotName));
    
    public static void MatchChildSnapshot<T>(this INotAfterTo<T> to, string childSnapshotName)
        => to.AddMatcher(obj => MatchChildSnapshotMatcher(obj, childSnapshotName));
    
    private static IMatcherResult MatchInlineSnapshotMatcher<T>(T obj, object expectedSnapshot) 
    {
        var snapper = SnapperFactory.GetJsonInlineSnapper(expectedSnapshot);
        var result = snapper.MatchSnapshot(obj);
        
        return new MatcherResult(result.Status is SnapResultStatus.SnapshotsMatch or SnapResultStatus.SnapshotUpdated,
            Messages.GetSnapResultMessage(result));
    }
    
    public static void MatchInlineSnapshot<T>(this ITo<T> to, object expectedSnapshot)
        => to.AddMatcher(obj => MatchInlineSnapshotMatcher(obj, expectedSnapshot));
    
    public static void MatchInlineSnapshot<T>(this IToAfterNot<T> to, object expectedSnapshot)
        => to.AddMatcher(obj => MatchInlineSnapshotMatcher(obj, expectedSnapshot));
    
    public static void MatchInlineSnapshot<T>(this INotAfterTo<T> to, object expectedSnapshot)
        => to.AddMatcher(obj => MatchInlineSnapshotMatcher(obj, expectedSnapshot));
}
