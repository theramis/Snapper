using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Xunit;

namespace Snapper.Tests;

public class SnapperCustomSettingsChildSnapshotsTests
{
    [Fact]
    public void SnapshotUsesDefaultSettings()
    {
        // Arrange
        var snapshot = new
        {
            TestValue = "value"
        };

        // Act
        snapshot.ShouldMatchChildSnapshot("childSnapshotName", SnapshotSettings.New());

        // Assert
        var filePath = Path.Combine(GetCurrentClassDirectory(), "_snapshots",
            $"{nameof(SnapperCustomSettingsChildSnapshotsTests)}_{nameof(SnapshotUsesDefaultSettings)}.json");
        var fileExists = File.Exists(filePath);
        Assert.True(fileExists);

        var snapshotContent = File.ReadAllText(filePath);
        Assert.Contains("childSnapshotName", snapshotContent);
    }

    [Fact]
    public void SnapshotUsesCustomSettings()
    {
        // Arrange
        var snapshot = new
        {
            TestValue = "value"
        };

        // Act
        var settings = SnapshotSettings.New()
            .SnapshotDirectory(Path.Combine(GetCurrentClassDirectory(), "_custom"))
            .SnapshotFileName("CustomFileNameForChildSnapshots")
            .SnapshotClassName("ThisClassNameWillNotBeUsed")
            .SnapshotTestName("ThisTestNameWillNotBeUsed");
        snapshot.ShouldMatchChildSnapshot("childSnapshotName", settings);

        // Assert
        var filePath = Path.Combine(GetCurrentClassDirectory(),
            "_custom", "CustomFileNameForChildSnapshots.json");
        var fileExists = File.Exists(filePath);
        Assert.True(fileExists);

        var snapshotContent = File.ReadAllText(filePath);
        Assert.Contains("childSnapshotName", snapshotContent);
    }

    [Fact]
    public void SnapshotUsesCustomTestAndClassNameSettingsWithStoreSnapshotPerClassFalse()
    {
        // Arrange
        var snapshot = new
        {
            TestValue = "value"
        };

        // Act
        var settings = SnapshotSettings.New()
            .SnapshotClassName("customClassNameForChildSnapshots")
            .SnapshotTestName("customTestName")
            .StoreSnapshotsPerClass(false);
        snapshot.ShouldMatchChildSnapshot("childSnapshotName", settings);

        // Assert
        var filePath = Path.Combine(GetCurrentClassDirectory(),
            "_snapshots", "customClassNameForChildSnapshots_customTestName.json");
        var fileExists = File.Exists(filePath);
        Assert.True(fileExists);

        var snapshotContent = File.ReadAllText(filePath);
        Assert.Contains("childSnapshotName", snapshotContent);
    }

    [Fact]
    public void SnapshotUsesCustomTestAndClassNameSettingsWithStoreSnapshotPerClassTrue()
    {
        // Arrange
        var snapshot = new
        {
            TestValue = "value"
        };

        // Act
        var settings = SnapshotSettings.New()
            .SnapshotClassName("customClassNameForChildSnapshot")
            .SnapshotTestName("customTestName")
            .StoreSnapshotsPerClass(true);
        snapshot.ShouldMatchChildSnapshot("childSnapshotName", settings);

        // Assert
        var filePath = Path.Combine(GetCurrentClassDirectory(),
            "_snapshots", "customClassNameForChildSnapshot.json");
        var fileExists = File.Exists(filePath);
        Assert.True(fileExists);

        var snapshotContent = File.ReadAllText(filePath);
        Assert.Contains("customTestName", snapshotContent);
        Assert.Contains("childSnapshotName", snapshotContent);
    }

    [Fact]
    public void SnapshotUsesCustomSerialiserSettings()
    {
        // Arrange
        var snapshot = new
        {
            TestValue = HttpStatusCode.OK
        };

        // Act
        var settings = SnapshotSettings.New()
            .SnapshotSerializerSettings(s =>
            {
                s.Converters.Add(new JsonStringEnumConverter());
            });
        snapshot.ShouldMatchChildSnapshot("childSnapshotName", settings);

        // Assert
        var filePath = Path.Combine(GetCurrentClassDirectory(), "_snapshots",
            $"{nameof(SnapperCustomSettingsChildSnapshotsTests)}_{nameof(SnapshotUsesCustomSerialiserSettings)}.json");
        // Assert
        var fileExists = File.Exists(filePath);
        Assert.True(fileExists);

        Assert.Contains("childSnapshotName", File.ReadAllText(filePath));
        Assert.Contains("OK", File.ReadAllText(filePath));
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
            snapshot.ShouldMatchChildSnapshot("childSnapshotName", settings);

            Assert.True(contains);
        }
        finally
        {
            SnapshotSettings.GlobalSnapshotSerialiserSettings = null;
        }
    }

    private static string GetCurrentClassDirectory([CallerFilePath] string callerFilePath = "")
        => Path.GetDirectoryName(callerFilePath);
}
