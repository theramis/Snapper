using System;
using System.Text;
using Snapper.Json;

namespace Snapper.Core;

internal static class Messages
{
    public static string GetSnapResultMessage(SnapResult result)
    {
        switch (result.Status)
        {
            case SnapResultStatus.SnapshotDoesNotExist:
                var message = new StringBuilder();
                message.AppendLine($"A snapshot does not exist.{Environment.NewLine}");
                message.AppendLine("Run the test outside of a CI environment to create a snapshot.");
                return message.ToString();
            case SnapResultStatus.SnapshotsMatch:
                return "Snapshots Match.";
            case SnapResultStatus.SnapshotsDoNotMatch:
                return JsonDiffGenerator.GetDiffMessage(result.OldSnapshot
                                                        ?? throw new InvalidOperationException()
                    , result.NewSnapshot);
            case SnapResultStatus.SnapshotUpdated:
                return "Snapshot was updated.";
            default:
                return string.Empty;
        }
    }

    public const string TestMethodNotFoundMessage
        = "A supported test method was not found. " +
          "Make sure you are using Snapper inside a supported test framework. " +
          "See https://theramis.github.io/Snapper/#/pages/faqs for more info.";

    public const string MalformedJsonSnapshotMessage
        = "The snapshot provided contains malformed JSON. See inner exception for details.";

    public const string UnableToDetermineTestFilePathMessage
        = "Unable to determine the file path of the test method. " +
          "Make sure optimisation of the test project is disabled. " +
          "See https://theramis.github.io/Snapper/#/pages/faqs for more info.";
}
