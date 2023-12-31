# Custom Snapshot Settings

Custom snapshot settings allows you to have greater control over your snapshots.
There are two main aspects to this feature.
1. It allows you to control how your snapshots are serialised and de-serialised
2. It allows you to control everything about the location of the snapshot file

## Customising Serialisation/Deserialisation

With custom snapshot settings you are given access to the `JsonSerializerSettings` from the `Newtonsoft.Json` library which is used internally by Snapper.

There are two ways for you to customise these settings.

### Global customisation

To update the serialiser settings globally you can set the following method.

```csharp
SnapshotSettings.GlobalSnapshotSerialiserSettings = (s) =>
{
    // Example: adding in the string enum converter
    s.Converters.Add(new StringEnumConverter());
};
```

You'll need to make sure the code above runs before your tests. Each test framework has it's own way of doing this.

### Customising per test

To update the serialiser settings per test you will need to pass an instance of `SnapshotSettings` to the assertion.

```csharp
var settings = SnapshotSettings.New()
    .SnapshotSerializerSettings(s =>
    {
        // Example: adding in the string enum converter
        s.Converters.Add(new StringEnumConverter());
    });

snapshot.ShouldMatchSnapshot(settings);
// OR
snapshot.ShouldMatchChildSnapshot("childSnapshotName", settings);
```

> Note: The per test customisation does NOT override the global one. The global one will be applied first and then the per test one.

## Customising Snapshot File Location

You can also use `SnapshotSettings` to customise how and where the snapshot file is created.

```csharp
public class MyTests
{
    public void MyFirstTest()
    {
        // Arrange
        var snapshot = new
        {
            TestValue = "value"
        };

        // Act/Assert
        var settings = SnapshotSettings.New()
            // Directory where the snapshot file is stored
            .SnapshotDirectory("C:/MySnapshotDir/_custom_")
            // Custom file name of the snapshot file
            .SnapshotFileName("CustomFileName")
            // Custom class name, used to make the file name if a custom file name is not provided
            .SnapshotClassName("CustomClassName")
            // Custom test name, used to make the file name if a custom file name is not provided
            .SnapshotTestName("CustomTestName")
            // Stores snapshots per class. Overrides any attributes.
            .StoreSnapshotsPerClass(true);
        snapshot.ShouldMatchSnapshot(settings);
    }
}
```

The best way to learn how these settings work is to have a look at some of the tests [here](https://github.com/theramis/Snapper/blob/master/project/Tests/Snapper.Tests/SnapperCustomSettingsTests.cs) or have a brief look at the logic [here](https://github.com/theramis/Snapper/blob/master/project/Snapper/Core/SnapshotIdResolver.cs#L23-L44) which should help you understand which settings are used when.

## Updating Snapshot using custom settings

You can set whether the snapshot should be updated or not.

```csharp
var settings = SnapshotSettings.New()
    .UpdateSnapshots(true);

// Snapshot will be updated
snapshot.ShouldMatchSnapshot(settings);
```