---
title: Basics
nav_order: 1
parent: Snapper
---

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

For `Theory` tests you should use the `ShouldMatchSnapshot(uniqueInstanceName)` extension method to match objects to a snapshot.
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
        // you need a pass in a unique instance name into the method.
        // Using the data passed into the method is generally.the way to go.
        obj.ShouldMatchSnapshot(value);
    }
}
```

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

For best practices in writing snapshot tests see [here](https://jestjs.io/docs/en/snapshot-testing#best-practices) for reccomendations from the jest team.

