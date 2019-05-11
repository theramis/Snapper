namespace Snapper.Core
{
    internal class SnapshotId
    {
        public string FilePath { get; }

        public string PrimaryId { get; }

        public string SecondaryId { get; }

        public SnapshotId(string filePath, string primaryId = null, string secondaryId = null)
        {
            FilePath = filePath;
            PrimaryId = primaryId;
            SecondaryId = secondaryId;
        }
    }
}
