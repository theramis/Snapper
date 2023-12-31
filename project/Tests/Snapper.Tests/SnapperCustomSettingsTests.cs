using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Xunit;

namespace Snapper.Tests;

public class SnapperCustomSettingsTests
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
        snapshot.ShouldMatchSnapshot(SnapshotSettings.New());

        // Assert
        var fileExists = File.Exists(Path.Combine(GetCurrentClassDirectory(), "_snapshots",
            $"{nameof(SnapperCustomSettingsTests)}_{nameof(SnapshotUsesDefaultSettings)}.json"));
        Assert.True(fileExists);
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
            .SnapshotFileName("CustomFileName")
            .SnapshotClassName("ThisClassNameWillNotBeUsed")
            .SnapshotTestName("ThisTestNameWillNotBeUsed");
        snapshot.ShouldMatchSnapshot(settings);

        // Assert
        var fileExists = File.Exists(Path.Combine(GetCurrentClassDirectory(),
            "_custom", "CustomFileName.json"));
        Assert.True(fileExists);
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
            .SnapshotClassName("customClassName")
            .SnapshotTestName("customTestName")
            .StoreSnapshotsPerClass(false);
        snapshot.ShouldMatchSnapshot(settings);

        // Assert
        var fileExists = File.Exists(Path.Combine(GetCurrentClassDirectory(),
            "_snapshots", "customClassName_customTestName.json"));
        Assert.True(fileExists);
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
            .SnapshotClassName("customClassName")
            .SnapshotTestName("customTestName")
            .StoreSnapshotsPerClass(true);
        snapshot.ShouldMatchSnapshot(settings);

        // Assert
        var filePath = Path.Combine(GetCurrentClassDirectory(),
            "_snapshots", "customClassName.json");
        var fileExists = File.Exists(filePath);
        Assert.True(fileExists);

        var snapshotContent = File.ReadAllText(filePath);
        Assert.Contains("\"TestValue\": \"value\"", snapshotContent);
        Assert.Contains("customTestName", snapshotContent);
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
        snapshot.ShouldMatchSnapshot(settings);

        // Assert
        var filePath = Path.Combine(GetCurrentClassDirectory(), "_snapshots",
            $"{nameof(SnapperCustomSettingsTests)}_{nameof(SnapshotUsesCustomSerialiserSettings)}.json");
        // Assert
        var fileExists = File.Exists(filePath);
        Assert.True(fileExists);

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
            snapshot.ShouldMatchSnapshot(settings);

            Assert.True(contains);
        }
        finally
        {
            SnapshotSettings.GlobalSnapshotSerialiserSettings = null;
        }
    }

    [Fact]
    public void SnapshotRespectsCustomUpdateSnapshots()
    {
        var filePath = Path.Combine(GetCurrentClassDirectory(), "_snapshots",
            $"{nameof(SnapperCustomSettingsTests)}_{nameof(SnapshotRespectsCustomUpdateSnapshots)}.json");

        // Arrange
        var snapshot = new
        {
            TestValue = "value"
        };
        snapshot.ShouldMatchSnapshot(); // Creates the initial snapshot
        snapshot = new
        {
            TestValue = "updated"
        };

        // Act
        var settings = SnapshotSettings.New()
            .UpdateSnapshots(true);
        snapshot.ShouldMatchSnapshot(settings);

        // Assert
        Assert.True(File.Exists(filePath));

        var snapshotContent = File.ReadAllText(filePath);
        Assert.Contains("\"TestValue\": \"updated\"", snapshotContent);

        if (File.Exists(filePath)) File.Delete(filePath);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SnapshotRespectsCustomUpdateSnapshotsOverEnvironmentVariable(bool shouldUpdate)
    {
        var filePath = Path.Combine(GetCurrentClassDirectory(), "_snapshots",
            $"{nameof(SnapperCustomSettingsTests)}_{nameof(SnapshotRespectsCustomUpdateSnapshotsOverEnvironmentVariable)}.json");

        // Arrange
        var snapshot = new
        {
            TestValue = "somevalue"
        };
        snapshot.ShouldMatchChildSnapshot($"{shouldUpdate}"); // Creates the initial snapshot

        snapshot = new
        {
            TestValue = "value"
        };
        Environment.SetEnvironmentVariable("UpdateSnapshots", $"{!shouldUpdate}");

        // Act
        var settings = SnapshotSettings.New()
            .UpdateSnapshots(shouldUpdate);
        var exception = Record.Exception(() => snapshot.ShouldMatchChildSnapshot($"{shouldUpdate}", settings));

        // Assert
        if (shouldUpdate)
        {
            Assert.Null(exception);
        }
        else
        {
            Assert.NotNull(exception);
            Assert.Equal("Snapper.Exceptions.SnapshotsDoNotMatchException", exception.GetType().FullName);
        }

        if (File.Exists(filePath)) File.Delete(filePath);
    }

    private static string GetCurrentClassDirectory([CallerFilePath] string callerFilePath = "")
        => Path.GetDirectoryName(callerFilePath);
}
