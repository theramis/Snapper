using System.Text.Json;
using FluentAssertions;
using Snapper.Json;
using Xunit;

namespace Snapper.Internals.Tests.Json
{
    public class JsonSnapshotSanitiserTests
    {
        private readonly JsonSnapshotSanitiser _sanitiser;

        public JsonSnapshotSanitiserTests()
        {
            _sanitiser = new JsonSnapshotSanitiser(SnapshotSettings.New());
            AssertionOptions.AssertEquivalencyUsing(opt => opt.ComparingByMembers<JsonElement>());
        }

        [Fact]
        public void SimpleJsonObjectTest()
        {
            var sanitisedObject = _sanitiser.SanitiseSnapshot(new
            {
                Key = "Value"
            });

            sanitisedObject.Should().BeEquivalentTo(JsonSerializer.SerializeToElement(new
            {
                Key = "Value"
            }));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2.1)]
        [InlineData(true)]
        [InlineData('a')]
        [InlineData("string")]
        public void PrimitivesTest(object obj)
        {
            var sanitisedObject = _sanitiser.SanitiseSnapshot(obj);

            sanitisedObject.Should().BeEquivalentTo(JsonSerializer.SerializeToElement(obj));
        }

        [Fact]
        public void ValidJsonStringTest()
        {
            var sanitisedObject = _sanitiser.SanitiseSnapshot("{ " +
                                                              "\"Key\" : \"value\"" +
                                                              "}");

            sanitisedObject.Should().BeEquivalentTo(JsonSerializer.SerializeToElement(new
            {
                Key = "value"
            }));
        }

        [Fact]
        public void InvalidJsonStringTest()
        {
            var sanitisedObject = _sanitiser.SanitiseSnapshot("{ " + "\"Key\" : \"value\"");
            sanitisedObject.Should()
                .BeEquivalentTo(
                    JsonSerializer.SerializeToElement("{ \"Key\" : \"value\"")
                );
        }

        [Fact]
        public void MalformedJsonStringTest()
        {
            var exception = Record.Exception(() => _sanitiser.SanitiseSnapshot("{ " +
                                                               "\"Key\" ======== \"value\"" +
                                                               "}"));

            exception.Should().BeNull();
        }
    }
}
