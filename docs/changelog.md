---
title: Changelog
nav_order: 99
---

# Changelog
All notable changes to the Snapper project.

## [2.1.0] - 2019-10-12
### Added
- Added the ability detect test methods when a test framework attribute is one of the parents of the attribute applied.

## [2.0.1] - 2019-09-11

### Bug Fix
- [Issue #27](https://github.com/theramis/Snapper/issues/27) Handled `System.NotImplementedException` when trying to determine which the calling test method.

## [2.0.0] - 2019-08-14

### Deprecated NuGet Packages
The following Nuget packages are deprecated. Bugs will still be fixed but no new features will be implemented in them.
- `Snapper.Core`
- `Snapper.Json`
- `Snapper.Json.Xunit`
- `Snapper.Json.Nunit`

### New NuGet Packages
- Introduced `Snapper` as the main nuget package. It provides all the functionality of `Snapper.Json` + `Snapper.Json.Xunit` as well as basic support for `NUnit`.
- Introduced `Snapper.Nunit` to provide extra `NUnit` specific features to Snapper. This package extends `NUnit's` constraints to match the functionality provided by deprecated package `Snapper.Json.Nunit`.

### Behaviour changes compared to V1
- Matching a snapshot previously created a json file based on the method name of the test. In V2 the json file is based on this format `{className}_{methodName}.json`.
- Matching a snapshot and passing in a snapshot name has been removed. A similar method has been introduced for having child snapshots. Useful for theory type tests.

### New Features
- **Child snapshots!** Child snapshots are a nice new way of creating snapshots for theory tests.
- **Inline snapshots!** Sometimes snapshots are small and it's not worth making a json file. Inline snapshots allow you to provide an object/string/json string which it can use as the snapshot.
- You can now use any object for snapshots. Previously an object that could not be converted into a JToken caused an exception. Now Snapper sanitises these objects so that they can be used in snapshots.

## [2.0.0-beta3] - 2019-05-26
### Added
- Updated `Snapper` and `Snapper.Nunit` to upload symbols to nuget symbols server

## [2.0.0-beta2] - 2019-05-26
### Added
- Updated `Snapper` and `Snapper.Nunit` to be strong named
- Updated `Snapper` and `Snapper.Nunit` to have sourcelink enabled

## [2.0.0-beta] - 2019-05-11

### Deprecated NuGet Packages
The following Nuget packages are deprecated. Bugs will still be fixed but no new features will be implemented in them.
- `Snapper.Core`
- `Snapper.Json`
- `Snapper.Json.Xunit`
- `Snapper.Json.Nunit`

### New NuGet Packages
- Introduced `Snapper` as the main nuget package. It provides all the functionality of `Snapper.Json` + `Snapper.Json.Xunit` as well as basic support for `NUnit`.
- Introduced `Snapper.Nunit` to provide extra `NUnit` specific features to Snapper. This package extends `NUnit's` constraints to match the functionality provided by deprecated package `Snapper.Json.Nunit`.

### Behaviour changes compared to V1
- Matching a snapshot previously created a json file based on the method name of the test. In V2 the json file is based on this format `{className}_{methodName}.json`.
- Matching a snapshot and passing in a snapshot name has been removed. A similar method has been introduced for having child snapshots. Useful for theory type tests.

### New Features
- **Child snapshots!** Child snapshots are a nice new way of creating snapshots for theory tests.
- **Inline snapshots!** Sometimes snapshots are small and it's not worth making a json file. Inline snapshots allow you to provide an object/string/json string which it can use as the snapshot.
- You can now use any object for snapshots. Previously an object that could not be converted into a JToken caused an exception. Now Snapper sanitises these objects so that they can be used in snapshots.

## [1.4.0] - 2019-05-11
### Added
- Added support for `.NET 45`

This will also be the final V1 version of Snapper.

## [1.3.1] - 2019-01-19
### Changed
- Downgraded `NUnit` dependency in `Snapper.Json.Nunit` to `3.6.0`

## [1.3.0] - 2019-01-19
### Added
- [Issue #3](https://github.com/theramis/Snapper/issues/3) Introduced support for NUnit via the `Snapper.Json.Nunit` nuget package. Thanks to [@fgather](https://github.com/fgather) for the contribution.

## [1.2.0] - 2018-6-5
### Changed
- Renamed `UpdateTestSnapshot` in `Snapper.Json.Xunit` to `UpdateSnapshots`.
- Updated `UpdateSnapshots` to be useable on methods and classes.

## [1.1.1] - 2018-4-2
### Changed
- Downgraded `xunit.extensibility.core` in `Snapper.Json.Xunit` to `2.3.0`

## [1.1.0] - 2018-4-2
### Changed
- Replace dependency on `xunit.assert.source` with `xunit.assert` in `Snapper.Json.Xunit`.
- Use `XUnitSnapper.MatchSnapshot()` to assert snapshots while using `Snapper.Json.Xunit`.

## [1.0.1] - 2018-3-19
### Changed
- Added `.netstandard 2` target to `Snapper.Json`

## [1.0.0] - 2018-3-17
The first stable release!

### Added
- **Snapper.Core**: Basic snapshot functionality. Stores snapshots in bytes. Use for extending Snapper.
- **Snapper.Json**: Extends Snapper.Core to provide storing snapshots in Json format
- **Snapper.Json.Xunit**: Extends Snapper.Json and integrates with the XUnit testing framework.

[2.0.0]: https://github.com/theramis/Snapper/compare/2.0.1...2.1.0
[2.0.0]: https://github.com/theramis/Snapper/compare/2.0.0...2.0.1
[2.0.0]: https://github.com/theramis/Snapper/compare/1.4.0...2.0.0
[2.0.0-beta3]: https://github.com/theramis/Snapper/compare/2.0.0-beta2...2.0.0-beta3
[2.0.0-beta2]: https://github.com/theramis/Snapper/compare/2.0.0-beta...2.0.0-beta2
[2.0.0-beta]: https://github.com/theramis/Snapper/compare/1.4.0...2.0.0-beta
[1.4.0]: https://github.com/theramis/Snapper/compare/1.3.1...1.4.0
[1.3.1]: https://github.com/theramis/Snapper/compare/1.3.0...1.3.1
[1.3.0]: https://github.com/theramis/Snapper/compare/1.2.0...1.3.0
[1.2.0]: https://github.com/theramis/Snapper/compare/1.1.1...1.2.0
[1.1.1]: https://github.com/theramis/Snapper/compare/1.1.0...1.1.1
[1.1.0]: https://github.com/theramis/Snapper/compare/1.0.1...1.1.0
[1.0.1]: https://github.com/theramis/Snapper/compare/1.0.0...1.0.1
[1.0.0]: https://github.com/theramis/Snapper/tree/1.0.0
