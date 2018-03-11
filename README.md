# Snapper (WIP)
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
- **Snapper.Json**: Extends Snapper.Core to provide storing snapshots in Json format
- **Snapper.Json.Xunit**: Extends Snapper.Json. Adds extensions to XUnit Assert.

Install the package through NuGet
```
nuget install <package_name>
```

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
This package extends XUnits `Assert` class and therefore conflicts with `xunit.assert` package.
You can safely remove the `xunit.assert` package and just use what is imported through this package.

```cs
// Snapshot name will be the same as the name of the test
Assert.MatchSnapshot(objectToSnapshot);

Assert.MatchSnapshot(snapshotName, objectToSnapshot);
```
To update snapshots set the Environment Variable `UpdateSnapshots` to `true` and run the tests.
You can also add the `[UpdateTestSnapshot]` attribute to your test and run it. (Remember to remove it before you commit your code)

## Todo
- ~~Write tests~~
- ~~Extend XUnit Assert e.g. `Assert.Snap(obj)` rather than `XUnitSnapper.Snap(obj)`~~
- Write wiki docs
- Write method/class docs
- ~~Update appveyor to build on every commit and publish nuget on tag~~
- Create sample project
- ~~Publish to Nuget~~
- ~~Add tags to Nuget~~
- Add logo to Nuget
- Downgrade project to lowest .net standard possible
- Downgrade nuget dependencies to lowest possible
