using System.Text.Json;
using System.Text.Json.Nodes;
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

            JsonNode.DeepEquals(sanitisedObject, JsonSerializer.SerializeToNode(new
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

            JsonNode.DeepEquals(sanitisedObject, JsonSerializer.SerializeToNode(obj));
        }

        [Fact]
        public void ValidJsonStringTest()
        {
            var sanitisedObject = _sanitiser.SanitiseSnapshot("{ " +
                                                              "\"Key\" : \"value\"" +
                                                              "}");

            JsonNode.DeepEquals(sanitisedObject, JsonSerializer.SerializeToNode(new
            {
                Key = "value"
            }));
        }

        [Fact]
        public void InvalidJsonStringTest()
        {
            var sanitisedObject = _sanitiser.SanitiseSnapshot("{ " + "\"Key\" : \"value\"");

            JsonNode.DeepEquals(sanitisedObject, JsonSerializer.SerializeToNode("{ \"Key\" : \"value\""));
        }

        [Fact]
        public void MalformedJsonStringTest()
        {
            var sanitisedObject = _sanitiser.SanitiseSnapshot("{ " +
                                                              "\"Key\" ======== \"value\"" +
                                                              "}");

            JsonNode.DeepEquals(sanitisedObject, JsonSerializer.SerializeToNode("{ " +
                "\"Key\" ======== \"value\"" +
                "}"));
        }
    }
}
