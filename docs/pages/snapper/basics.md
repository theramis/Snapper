# Basics

Snapper allows you to create snapshots of objects in a jest-esque fashion. It allows you to do gold master testing easily in C#.

*Note: The terminology used on this page will be using xUnits test types i.e. `Fact` & `Theory` methods. The concepts apply to the equivalent types in other testing frameworks.*


For `Fact` tests you should use the `ShouldMatchSnapshot()` extension method to match objects to a snapshot.

```csharp
public class MyTestClass {

    [Fact]
    public void MyTest(){
        var obj = new {
            Key = "value"
        };

        obj.ShouldMatchSnapshot();
    }
}
```

## Child Snapshots

For `Theory` tests you should use the `ShouldMatchChildSnapshot(childSnapshotName)` extension method to match objects to a child snapshot.
```csharp
public class MyTestClass {

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void MyTest(int value){
        var obj = new {
            Key = value
        };

        // Since the snapshot for each test instance will be different
        // you need a pass in a child snapshot name into the method.
        // Using the data passed into the test method is generally a nice
        // child snapshot name
        obj.ShouldMatchChildSnapshot($"child_{value}");
    }
}
```
The idea of child snapshots is new in Snapper V2. A child snapshot allows you to have 1 snapshot file for 1 test but multiple child snapshots. This is super valuable for theory tests as its 1 test with different values.

The snapshot for the above test would look like this.
```
{
    "child_1": {
        "Key": 1
    },
    "child_2": {
        "Key": 1
    },
    "child_3": {
        "Key": 1
    }
}
```

## Inline Snapshots

Sometimes the object you want to snapshot is not huge and nice to be able to have the snapshot inline. You can use the `ShouldMatchInlineSnapshot()` extension method to match objects to an inline snapshot.
```csharp
public class MyTestClass {

    [Fact]
    public void MyTest(){
        var obj = new {
            Key = "value"
        };

        obj.ShouldMatchInlineSnapshot("{ \"Key\": \"value\"}");
    }
}
```

For more examples see [here](https://github.com/theramis/Snapper/tree/master/project/Tests/Snapper.Tests).

For best practices in writing snapshot tests see [here](https://jestjs.io/docs/en/snapshot-testing#best-practices) for recommendations from the jest team.

