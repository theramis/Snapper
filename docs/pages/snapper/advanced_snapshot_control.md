# Advanced Snapshot File Control

> This is currently an experimental feature available from v2.2.4 onwards

Sometimes you want to have more control over how the snapshot file looks and where it should be created. 

You can pass in an instance of the [SnapshotId](https://github.com/theramis/Snapper/blob/master/project/Snapper/Core/SnapshotId.cs) class to the `ShouldMatchSnapshot()` method as seen in the example below.

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
        snapshot.ShouldMatchSnapshot(new SnapshotId(
            "C:\\MyTestProject\\mysnapshotsfolder",     // Directory where the snapshot file is stored
            nameof(MyTests),                            // Class Name
            nameof(MyFirstTest),                        // Method Name
            "childSnapshotName",                        // Child Snapshot Name
            storeSnapshotsPerClass: false));            // Store Snapshots per class
    }
}
```

This will create the following snapshot file `C:\MyTestProject\mysnapshotsfolder\MyTests.json` with this content. 

```json
{
    "MyFirstTest": {
        "childSnapshotName": {
            "TestValue": "value"
        }
    }
}
```

## How does Snapper use SnapshotId to create the snapshot

`SnapshotId` has five different parameters it takes in and they are used to determine the details of the snapshot file. 

The directory where the snapshot file is stored is one of the parameters passed in. 

The name of the snapshot file changes depending on the `storeSnapshotsPerClass` parameter. If it is `true` then the name of the snapshot file is the value of the `className` parameter. If it is `false` then the name of the snapshot file is a combination of the `className` and `methodName` parameters.

The `childSnapshotName` parameter is used to decide if the snapshot has a child snapshot or not. See [Child Snapshots](/pages/snapper/basics?id=child-snapshots) for more details.

The `storeSnapshotsPerClass` parameter is essentially the same as the `[StoreSnapshotsPerClass]` attribute. See [Snapshots per class](/pages/snapper/snapshots_per_class?id=snapshots-per-class) for more info.

See [SnapshotId](https://github.com/theramis/Snapper/blob/master/project/Snapper/Core/SnapshotId.cs) for the actual implementation which should be easy to understand.

### Examples

```csharp
// Creates `C:\snaps\class_method.json`
snapshot.ShouldMatchSnapshot(new SnapshotId("C:\\snaps", "class", "method", "child", false);
/*
{
    "child": {
        ...
    }
}
*/

// Creates `C:\snaps\class_method.json`
snapshot.ShouldMatchSnapshot(new SnapshotId("C:\\snaps", "class", "method", null, false);
/*
{
    ...
}
*/

// Creates `C:\snaps\class.json`
snapshot.ShouldMatchSnapshot(new SnapshotId("C:\\snaps", "class", "method", null, true);
/*
{
    "method": {
        ...
    }
}
*/

// Creates `C:\snaps\class.json`
snapshot.ShouldMatchSnapshot(new SnapshotId("C:\\snaps", "class", "method", "child", true);
/*
{
    "method": {
        "child": {
            ...
        }
    }
}
*/
```