﻿using System;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snapper.Exceptions;
using Snapper.Json;
using Xunit;

namespace Snapper.Internals.Tests.Json
{
    public class JsonSnapshotSanitiserTests
    {
        private readonly JsonSnapshotSanitiser _sanitiser;

        public JsonSnapshotSanitiserTests()
        {
            _sanitiser = new JsonSnapshotSanitiser();
        }

        [Fact]
        public void SimpleJsonObjectTest()
        {
            var sanitisedObject = _sanitiser.SanitiseSnapshot(new
            {
                Key = "Value"
            });

            sanitisedObject.Should().BeEquivalentTo(new
            {
                Key = "Value"
            });
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

            sanitisedObject.Should().BeEquivalentTo(new
            {
                AutoGenerated = obj
            });
        }

        [Fact]
        public void ValidJsonStringTest()
        {
            var sanitisedObject = _sanitiser.SanitiseSnapshot("{ " +
                                                              "\"Key\" : \"value\"" +
                                                              "}");

            sanitisedObject.Should().BeEquivalentTo(JObject.FromObject(new
            {
                Key = "value"
            }));
        }

        [Fact]
        public void InvalidJsonStringTest()
        {
            var sanitisedObject = _sanitiser.SanitiseSnapshot("{ " +
                                                              "\"Key\" : \"value\"");
            sanitisedObject.Should().BeEquivalentTo(new
            {
                AutoGenerated = "{ \"Key\" : \"value\""
            });
        }

        [Fact]
        public void MalformedJsonStringTest()
        {
            var exception = Record.Exception(() => _sanitiser.SanitiseSnapshot("{ " +
                                                               "\"Key\" ======== \"value\"" +
                                                               "}"));

            exception.Should().NotBeNull();
            exception.GetType().FullName.Should().Be(typeof(MalformedJsonSnapshotException).FullName);
            exception.Message.Should()
                .Be("The snapshot provided contains malformed JSON. See inner exception for details.");
        }

        [Fact]
        public void DateTimeOffsetStringTestWithSerializationSettings()
        {
            SnapperSettings.SnapshotDeserializationSettings = () => new JsonSerializerSettings()
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateParseHandling = DateParseHandling.DateTimeOffset,
            };

            var sanitisedObject = _sanitiser.SanitiseSnapshot("{" +
                                        "\"Key\" : \"2010-12-31T00:00:00+00:00\"" +
                                        "}").ToString();

            sanitisedObject.Should()
                .Be($"{{{Environment.NewLine}" +
                    "  \"Key\": \"2010-12-31T00:00:00+00:00\"" +
                    $"{Environment.NewLine}}}");
        }
    }
}
