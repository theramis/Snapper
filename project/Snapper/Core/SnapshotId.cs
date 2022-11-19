namespace Snapper.Core;

internal record SnapshotId(string FilePath, string? PrimaryId, string? SecondaryId);
