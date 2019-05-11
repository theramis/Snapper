---
title: Storing Snapshots per class
nav_order: 3
parent: Snapper
---

# Snapshots per class

By default Snapper stores a snapshot for each test method into a different file. This is sometimes not ideal when snapshots for a method are small or as you can end up with a large number of snapshot files.

You can apply the `[StoreSnapshotsPerClass]` attribute on a class or assembly and one snapshot file be created for a class.

### Example
```csharp
[StoreSnapshotsPerClass]
public class MyTestClass {

    [Fact]
    public void MyFirstTest(){
        var obj = new {
            Key = "value1"
        };

        obj.ShouldMatchSnapshot();
    }

    [Fact]
    public void MySecondTest(){
        var obj = new {
            Key = "value2"
        };

        obj.ShouldMatchSnapshot();
    }
}
```
The test class above would generate a snapshot file with the following json.
```json
{
    "MyFirstTest" : {
        "Key": "value1"
    },
    "MySecondTest" : {
        "Key": "value2"
    },
}

```