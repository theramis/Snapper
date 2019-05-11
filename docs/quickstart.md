---
title: Quickstart
nav_order: 1
---

# Quickstart

## Prerequisites
To run this quickstart, you need the following prerequisites:

- NuGet/Visual Studio 2017/Rider
- Platform which supports .NET standard 2.0. e.g. .NET Core 2.0+

## Steps

1. Install the latest NuGet package for `Snapper` to your project.
```
nuget install Snapper
```

2. Usage inside of a xUnit test method
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
The above code will try match the `obj` variable with a snapshot in the file `_snapshots/MyTestClass_MyTest.json` (relative to the file in which `MyTestClass` exists).
For a new test this will fail as the file does not exist with an error similiar to this.
```
    Snapper.Exceptions.SnapshotDoesNotExistException : A snapshot does not exist.

    Apply the [UpdateSnapshots] attribute on the test method or class and then run the test again to create a snapshot.
```

3. Lets create the snapshot so that the test passes. Apply the `[UpdateSnapshots]` attribute on the method and run the test again.
```csharp
public class MyTestClass {

    [UpdateSnapshots]
    [Fact]
    public void MyTest(){
        var obj = new {
            Key = "value"
        };

        obj.ShouldMatchSnapshot();
    }
}
```

4. A file called `_snapshots/MyTestClass_MyTest.json` should have been created with the following content.
```json
{
  "Key": "value"
}
```
You can now remove the `[UpdateSnapshots]` attribute on the method. You should also commit this json file with your source code.

5. Lets make our test fail. Update your code to the following.
```csharp
public class MyTestClass {

    [Fact]
    public void MyTest(){
        var obj = new {
            Key = "My new value"
        };

        obj.ShouldMatchSnapshot();
    }
}
```
Run the test and you shall see an error message similiar to this.
```
    Snapper.Exceptions.SnapshotsDoNotMatchException :
    Snapshots do not match
    - Snapshot
    + Received


    {
    -    "Key": "value"
    +    "Key": "My new value"
    }
```
You can then update the snapshot using the `[UpdateSnapshots]` attribute or fix the failing test.

For more examples see [here](https://github.com/theramis/Snapper/tree/master/project/Tests/Snapper.Tests).