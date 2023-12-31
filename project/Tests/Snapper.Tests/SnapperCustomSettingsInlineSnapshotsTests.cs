using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using Xunit;

namespace Snapper.Tests;

public class SnapperCustomSettingsInlineSnapshotTests
{
    [Fact]
    public void SnapshotUsesDefaultSettings()
    {
        // Arrange
        var snapshot = new
        {
            TestValue = "value"
        };

        // Act/Assert
        snapshot.ShouldMatchInlineSnapshot(new
        {
            TestValue = "value"
        }, SnapshotSettings.New());
    }

    [Fact]
    public void SnapshotUsesCustomSerialiserSettings()
    {
        // Arrange
        var snapshot = new
        {
            TestValue = HttpStatusCode.OK
        };

        // Act/Assert
        var settings = SnapshotSettings.New()
            .SnapshotSerializerSettings(s =>
            {
                s.Converters.Add(new JsonStringEnumConverter());
            });
        snapshot.ShouldMatchInlineSnapshot("{ \"TestValue\":\"OK\"}", settings);
    }

    [Fact]
    public void SnapshotAppliesGlobalCustomSerialiserSettingsFirst()
    {
        // Arrange
        var snapshot = new
        {
            TestValue = HttpStatusCode.OK
        };
        // Setup Global Settings
        SnapshotSettings.GlobalSnapshotSerialiserSettings = (s) =>
        {
            s.Converters.Add(new JsonStringEnumConverter());
        };

        // Act
        try
        {
            var contains = false;
            var settings = SnapshotSettings.New()
                .SnapshotSerializerSettings(s =>
                {
                    // this should be called later on and proves that the global one was applied first
                    contains = s.Converters.Any(c => c.GetType() == typeof(JsonStringEnumConverter));
                });
            snapshot.ShouldMatchInlineSnapshot("{ \"TestValue\":\"OK\"}", settings);

            Assert.True(contains);
        }
        finally
        {
            SnapshotSettings.GlobalSnapshotSerialiserSettings = null;
        }
    }
}
