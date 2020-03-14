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
2. Create an xUnit test like shown
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
3. Lets create the snapshot so that the test passes. We could create the snapshot file manually if we wanted but that's a bit annoying. Luckily Snapper can generate a snapshot file for us! Apply the `[UpdateSnapshots]` attribute on the method and run the test again.
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
You can now remove the `[UpdateSnapshots]` attribute on the method. You should also commit the `_snapshots/MyTestClass_MyTest.json` snapshot file with your source code.

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
You can then update the snapshot by adding the `[UpdateSnapshots]` attribute to the test and running it again or if the change was invalid fix the failing test.
<br></br>
For more examples of tests written using Snapper see [here](https://github.com/theramis/Snapper/tree/master/project/Tests/Snapper.Tests).