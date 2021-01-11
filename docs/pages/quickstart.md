# Quickstart

## Prerequisites
To run this quickstart, you need the following prerequisites:

- An IDE such as VS 2019/Rider or your favourite text editor (e.g VS Code)
- Platform which supports .NET standard 2.0. e.g. .NET Core 2.0+
- Nuget
- Basic knowledge of how to write tests in C#

## Steps

1. Install the latest NuGet package for `Snapper` to your project.
```bash
nuget install Snapper
```
1. Create an xUnit test like shown
```csharp
public class MyTestClass {

    [Fact]
    public void MyTest()
    {
        var obj = new {
            Key = "value"
        };

        obj.ShouldMatchSnapshot();
    }
}
```
Run the test and you'll see that it passes.
The above code will try match the `obj` variable with a snapshot in the file `_snapshots/MyTestClass_MyTest.json` (relative to the file in which `MyTestClass` exists).
For a new test the snapshot file would not exist yet so Snapper automatically creates it on the first run.
> Snapper automatically creates a new snapshot file for a new test from version v2.3.0 onwards. For previous versions use the `[UpdateSnapshots]` attribute to generate your initial snapshot.

1. A file called `_snapshots/MyTestClass_MyTest.json` should have been created with the following content.
```json
{
  "Key": "value"
}
```
You should commit the `_snapshots/MyTestClass_MyTest.json` snapshot file with your source code.

1. Lets make our test fail due to a change in requirements. Update your code to the following.
```csharp
public class MyTestClass {

    [Fact]
    public void MyTest()
    {
        var obj = new {
            Key = "My new value"
        };

        obj.ShouldMatchSnapshot();
    }
}
```
Run the test and you will see a nice error message showing the difference between the snapshot and the new object.
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

1. Once you've verified the new snapshot is expected you can update the snapshot by appling the `[UpdateSnapshots]` attribute on the method and then running the test again.
```csharp
public class MyTestClass {

    [UpdateSnapshots]
    [Fact]
    public void MyTest()
    {
        var obj = new {
            Key = "My new value"
        };

        obj.ShouldMatchSnapshot();
    }
}
```
This will update the snapshot file with the latest snapshot. Remember to remove the `[UpdateSnapshots]` attribute before you commit your code!
<br></br>
For more examples of tests written using Snapper see [here](https://github.com/theramis/Snapper/tree/master/project/Tests/Snapper.Tests).