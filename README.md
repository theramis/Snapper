# Snapper
**Bringing Jest-esque Snapshot testing to C#**

[![AppVeyor](https://img.shields.io/appveyor/ci/gruntjs/grunt.svg?style=for-the-badge)](https://ci.appveyor.com/project/theramis/snapper)
[![GitHub release](https://img.shields.io/github/release/theramis/snapper.svg?style=for-the-badge)](https://github.com/theramis/Snapper)
[![NuGet](https://img.shields.io/nuget/v/Snapper.Core.svg?style=for-the-badge)](https://www.nuget.org/packages/Snapper.Core)
[![Documentation WIP](https://img.shields.io/badge/Docs-WIP-red.svg?style=for-the-badge)](https://github.com/theramis/Snapper)

Snapper is a [NuGet library](https://www.nuget.org/packages/Snapper.Core) which captures snapshots of objects to simplify testing.
It is very heavily based on Jest Snapshot Testing.

## Getting Started
Currently Snapper consists of three different NuGet packages for extensibility.

Choose the package which best fits your needs
- **Snapper.Core**: Basic snapshot functionality. Stores snapshots in bytes. Use for extending Snapper.
- **Snapper.Json**: Extends `Snapper.Core` to provide storing snapshots in Json format
- **Snapper.Json.Xunit**: Extends `Snapper.Json` and integrates with the XUnit testing framework.
- **Snapper.Json.Nunit**: Extends `Snapper.Json` and integrates with the NUnit testing framework.

Install the package through NuGet
```
nuget install <package_name>
```
## Snapper v2
I'm currently working version 2 of Snapper. It's a work in progress at the moment. If you have any thoughts/suggestions/concerns about the changes please get in touch by creating an issue.

Current thoughts/progress on what will be in V2:
- https://github.com/theramis/Snapper/issues/13
- https://github.com/theramis/Snapper/issues/12
- Deprecate `Snapper.Core`, `Snapper.Json`, `Snapper.Json.Xunit` and `Snapper.Json.Nunit` nuget packages (Please create an issue if this is a concern)
- Introduce a new nuget package called `Snapper` which would provide the core functionality for all supported frameworks. (Xunit and Nunit initially)
- Introduce `Snapper.Xunit` and `Snapper.Nunit`as a replacement to the deprecated nuget packages. These packages would provide some extra features which are specific to the XUnit and NUnit frameworks.


## Using Snapper

### Snapper.Core

```cs
// Create class which implements IAssert
var asserter = new ClassWhichImplementsIAssert();

var snapper = new Snapper(asserter, directoryToStoreSnapshots);

// the object to snapshot must be marked as `Serializable`
snapper.Snap("snapshotName", objectToSnapshot);
```
To update snapshots set the Environment Variable `UpdateSnapshots` to `true` and run the tests.

### Snapper.Json

```cs
// Create class which implements IAssert
var asserter = new ClassWhichImplementsIAssert();

var snapper = new JsonSnapper(asserter, directoryToStoreSnapshots);

snapper.Snap("snapshotName", objectToSnapshot);
```
To update snapshots set the Environment Variable `UpdateSnapshots` to `true` and run the tests.

### Snapper.Json.Xunit
This package extends `Snapper.Json` to provide integration with the `XUnit` testing framework.

```cs
// Snapshot name will be the same as the name of the test
XUnitSnapper.MatchSnapshot(objectToSnapshot);

XUnitSnapper.MatchSnapshot(snapshotName, objectToSnapshot);
```
To update snapshots set the Environment Variable `UpdateSnapshots` to `true` and run the tests.
You can also add the `[UpdateSnapshots]` attribute to your test method/class and run it. (Remember to remove it before you commit your code)

### Snapper.Json.Nunit
This package extends `Snapper.Json` to provide integration with the `NUnit` testing framework.

```cs
// Snapshot name will be the same as the name of the test
Assert.That(objectToSnapshot, Is.EqualToSnapshot());

Assert.That(objectToSnapshot, Is.EqualToSnapshot(snapshotName));
```
To update snapshots set the Environment Variable `UpdateSnapshots` to `true` and run the tests.
You can also add the `[UpdateSnapshots]` attribute to your test method/class and run it. (Remember to remove it before you commit your code)

## FAQS

### Why am I getting an `InvalidOperation` exception saying Snapper is not being used inside a test method when using XUnitSnapper/NUnitSnapper only when in a Release build?
Snapper uses the [StackTrace](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.stacktrace?view=netstandard-2.0) to determine which test method it was called from. In a Release build the compiler optimises the code which can cause some issues with how Snapper determines the test method. The compiler in this case is inlining the method which makes the method invisible to the StackTrace class. See [link](https://stackoverflow.com/questions/3924995/what-is-method-inlining) for a better explanation.

There are currently two solution for this issue.
1. Set the following attribute on the test method `[MethodImpl(MethodImplOptions.NoInlining)]` as seen [here](https://github.com/theramis/Snapper/blob/516598b41426fcfd0968db170dcd805e30604cbb/project/Tests/Snapper.Tests/SnapperSnapshotsPerClassTests.cs#L13).
2. Disable optimisation of code for your release builds of your test project. You can do this by adding this into your projects csproj file.
```
<PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
    <Optimize>false</Optimize>
</PropertyGroup>
```

For more information see this issue: https://github.com/theramis/Snapper/issues/16

## Todo
- ~~Write tests~~
- ~~Extend XUnit Assert e.g. `Assert.Snap(obj)` rather than `XUnitSnapper.Snap(obj)`~~
- Write wiki docs
- ~~Update appveyor to build on every commit and publish nuget on tag~~
- Create sample project
- ~~Publish to Nuget~~
- ~~Add tags to Nuget~~
- Add logo to Nuget
- ~~Downgrade project to lowest .net standard possible~~
- ~~Downgrade nuget dependencies to lowest possible~~
- Create PR and Issue templates
- Write tests for testing json store. Use goldmaster testing
- Write tests for json diff message maker
- Update nuget package descriptions for xunit and nunit to say they have extra features
- Figure out all of the .netstandards to target and lowest nuget package versions
- Make tests fail if update var is on in CI
- mention feature of using a string which is json
