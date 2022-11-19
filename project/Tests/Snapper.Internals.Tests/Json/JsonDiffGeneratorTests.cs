using System;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Snapper.Core;
using Snapper.Json;
using Xunit;

namespace Snapper.Internals.Tests.Json
{
    public class JsonDiffGeneratorTests
    {
        [Fact]
        public void ValueChangedTest()
        {
            var currentSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value"
            });

            var newSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value2"
            });

            var result = JsonDiffGenerator.GetDiffMessage(currentSnapshot, newSnapshot);

            result.Should().Be(DiffMaker(builder =>
            {
                builder.AppendLine("{");
                builder.AppendLine("-    \"Key\": \"Value\"");
                builder.AppendLine("+    \"Key\": \"Value2\"");
                builder.AppendLine("}");
            }));
        }

        [Fact]
        public void ValueRemovedTest()
        {
            var currentSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value",
                Key2 = "Value"
            });

            var newSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value"
            });

            var result = JsonDiffGenerator.GetDiffMessage(currentSnapshot, newSnapshot);

            result.Should().Be(DiffMaker(builder =>
            {
                builder.AppendLine("{");
                builder.AppendLine("-    \"Key\": \"Value\",");
                builder.AppendLine("-    \"Key2\": \"Value\"");
                builder.AppendLine("+    \"Key\": \"Value\"");
                builder.AppendLine("}");
            }));
        }

        [Fact]
        public void ValueAddedTest()
        {
            var currentSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value"
            });

            var newSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value",
                Key2 = "Value"
            });

            var result = JsonDiffGenerator.GetDiffMessage(currentSnapshot, newSnapshot);

            result.Should().Be(DiffMaker(builder =>
            {
                builder.AppendLine("{");
                builder.AppendLine("-    \"Key\": \"Value\"");
                builder.AppendLine("+    \"Key\": \"Value\",");
                builder.AppendLine("+    \"Key2\": \"Value\"");
                builder.AppendLine("}");
            }));
        }

        [Fact]
        public void ValueAddedTest_LargeObject()
        {
            var currentSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value",
                Key1 = "Value",
                Key2 = "Value",
                Key3 = "Value",
                Key4 = "Value",
                Key5 = "Value",
            });

            var newSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value",
                Key1 = "Value",
                Key2 = "Value",
                Key3 = "Value",
                NewKey = "Value",
                Key4 = "Value",
                Key5 = "Value",
            });

            var result = JsonDiffGenerator.GetDiffMessage(currentSnapshot, newSnapshot);

            result.Should().Be(DiffMaker(builder =>
            {
                builder.AppendLine("  \"Key2\": \"Value\",");
                builder.AppendLine("  \"Key3\": \"Value\",");
                builder.AppendLine("+    \"NewKey\": \"Value\",");
                builder.AppendLine("  \"Key4\": \"Value\",");
                builder.AppendLine("  \"Key5\": \"Value\"");
            }));
        }

        [Fact]
        public void ValueRemovedTest_LargeObject()
        {
            var currentSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value",
                Key1 = "Value",
                Key2 = "Value",
                Key3 = "Value",
                Key4 = "Value",
                Key5 = "Value",
            });

            var newSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value",
                Key1 = "Value",
                Key2 = "Value",
                Key4 = "Value",
                Key5 = "Value",
            });

            var result = JsonDiffGenerator.GetDiffMessage(currentSnapshot, newSnapshot);

            result.Should().Be(DiffMaker(builder =>
            {
                builder.AppendLine("  \"Key1\": \"Value\",");
                builder.AppendLine("  \"Key2\": \"Value\",");
                builder.AppendLine("-    \"Key3\": \"Value\",");
                builder.AppendLine("  \"Key4\": \"Value\",");
                builder.AppendLine("  \"Key5\": \"Value\"");
            }));
        }

        [Fact]
        public void ValueChangedTest_LargeObject()
        {
            var currentSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value",
                Key1 = "Value",
                Key2 = "Value",
                Key3 = "Value",
                Key4 = "Value",
                Key5 = "Value",
            });

            var newSnapshot = MakeJsonSnapshot(new
            {
                Key = "Value",
                Key1 = "Value",
                Key2 = "Value",
                Key3 = "NewValue",
                Key4 = "Value",
                Key5 = "Value",
            });

            var result = JsonDiffGenerator.GetDiffMessage(currentSnapshot, newSnapshot);

            result.Should().Be(DiffMaker(builder =>
            {
                builder.AppendLine("  \"Key1\": \"Value\",");
                builder.AppendLine("  \"Key2\": \"Value\",");
                builder.AppendLine("-    \"Key3\": \"Value\",");
                builder.AppendLine("+    \"Key3\": \"NewValue\",");
                builder.AppendLine("  \"Key4\": \"Value\",");
                builder.AppendLine("  \"Key5\": \"Value\"");
            }));
        }

        private static string DiffMaker(Action<StringBuilder> addLinesFunc)
        {
            var diff = new StringBuilder(Environment.NewLine);
            diff.AppendLine("Snapshots do not match");
            diff.AppendLine("- Snapshot");
            diff.AppendLine("+ Received");
            diff.AppendLine(Environment.NewLine);
            addLinesFunc.Invoke(diff);
            return diff.ToString();
        }

        private JsonSnapshot MakeJsonSnapshot(object val)
            => new JsonSnapshot(DummySnapshotId(), JObject.FromObject(val));

        private static SnapshotId DummySnapshotId()
            => new SnapshotId("dir", "filename", "testname", false);
    }
}
