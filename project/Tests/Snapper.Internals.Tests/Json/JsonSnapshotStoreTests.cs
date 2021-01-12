using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Snapper.Core;
using Snapper.Json;
using Xunit;

namespace Snapper.Internals.Tests.Json
{
    public class JsonSnapshotStoreTests
    {
        public enum MyTestEnum
        {
            FirstValue = 0,
            SecondValue = 1,
            LastValue = 99
        }

        private readonly JsonSnapshotStore _snapshotStore;

        public JsonSnapshotStoreTests() => _snapshotStore = new JsonSnapshotStore();

        [Fact]
        public void LoadingNonExistingSnapshot()
        {
            var snapshot = _snapshotStore.GetSnapshot(new SnapshotId(".", "not-there", "neither"));

            snapshot.Should().BeNull();
        }

        [Fact]
        public void StoreAndLoadSnapshotWithCustomizedSettings()
        {
            var snapshotId = new SnapshotId(".", nameof(JsonSnapshotStoreTests), nameof(StoreAndLoadSnapshotWithCustomizedSettings));
            var fancyState = new {MyProp = MyTestEnum.FirstValue, OtherProp = MyTestEnum.LastValue, SomeString = "Nice ..."};

            _snapshotStore.StoreSnapshot(snapshotId, fancyState);

            var result = _snapshotStore.GetSnapshot(snapshotId);

            var expected = JObjectHelper.FromObject(fancyState);
            expected.Value<string>("MyProp").Should().Be(nameof(MyTestEnum.FirstValue));
            expected.Value<string>("OtherProp").Should().Be(nameof(MyTestEnum.LastValue));
            result.Should().BeEquivalentTo(expected);
        }

        public class CustomSnapperSettings : SnapperSettings
        {
            protected override JsonSerializerSettings Customize(JsonSerializerSettings defaultSettings)
            {
                defaultSettings.Converters = new List<JsonConverter> {new StringEnumConverter()};
                return defaultSettings;
            }
        }
    }
}
