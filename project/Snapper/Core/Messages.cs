using System;
using System.Text;
using Snapper.Attributes;
using Snapper.Json;

namespace Snapper.Core
{
    internal static class Messages
    {
        public static string GetSnapResultMessage(SnapResult result)
        {
            switch (result.Status)
            {
                case SnapResultStatus.SnapshotDoesNotExist:
                    var attributeName = nameof(UpdateSnapshotsAttribute).Replace("Attribute", string.Empty);
                    var message = new StringBuilder();
                    message.AppendLine($"A snapshot does not exist.{Environment.NewLine}");
                    message.AppendLine($"Apply the [{attributeName}] attribute on the " +
                                       $"test method or class and then run the test again to " +
                                       $"create a snapshot.");
                    return message.ToString();
                case SnapResultStatus.SnapshotsMatch:
                    return "Snapshots Match.";
                case SnapResultStatus.SnapshotsDoNotMatch:
                    return JsonDiffGenerator.GetDiffMessage(result.OldSnapshot, result.NewSnapshot);
                case SnapResultStatus.SnapshotUpdated:
                    return "Snapshot was updated.";
                default:
                    return string.Empty;
            }
        }

        // TODO add supported test frameworks in docs and add link to troubleshooting
        // mention inlining
        public const string TestMethodNotFoundMessage
            = "A supported test method was not found. " +
               "Make sure you are using Snapper inside a supported test framework. " +
               "See <link> for troubleshooting tips.";

        public const string InvalidJsonInlineSnapshotMessage
            = "The inline snapshot provided contains invalid JSON. See inner exception for details.";
    }
}
