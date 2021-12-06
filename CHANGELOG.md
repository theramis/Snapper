# Changelog
All notable changes to the Snapper project.

## [2.3.1] - 2021-12-06
### Added
- [PR #86](https://github.com/theramis/Snapper/pull/86) Added support for NUnit `TestCase` and `TestCaseSource` attributes. Thanks to [@jaytulk](https://github.com/jaytulk) for the contribution.

## [2.3.0] - 2021-01-11
### Added
- [Issue #69](https://github.com/theramis/Snapper/issues/69) [PR #71](https://github.com/theramis/Snapper/pull/71) Snapshots are now automatically generated on the first test run if the snapshot file does not exist.
- [Issue #57](https://github.com/theramis/Snapper/issues/57) [PR #72](https://github.com/theramis/Snapper/pull/72) Introduced `Matches.Snapshot()` and `Matches.ChildSnapshot()` to `Snapper.Nunit` nuget package. Thanks to [@lilasquared](https://github.com/lilasquared) for the contribution.

### Deprecated
- [Issue #57](https://github.com/theramis/Snapper/issues/57) [PR #72](https://github.com/theramis/Snapper/pull/72) Deprecated `Snapper.Nunit.Is.EqualToSnapshot()` in `Snapper.Nunit`. This will be removed in Snapper V3. Thanks to [@lilasquared](https://github.com/lilasquared) for the contribution.

## [2.2.4] - 2020-03-16
### Added
- [PR #60](https://github.com/theramis/Snapper/pull/60) **Experimental Feature**: Advanced snapshot file control! Get more control over how and where the snapshot file is created. See [here](https://theramis.github.io/Snapper/#/pages/snapper/advanced_snapshot_control) for more details.
Fixes the following issues: [#30](https://github.com/theramis/Snapper/issues/30) [#48](https://github.com/theramis/Snapper/issues/48) [#24](https://github.com/theramis/Snapper/issues/24).
- [PR #58](https://github.com/theramis/Snapper/pull/58) `Snapper` can now detect the Azure DevOps CI Environment. Thanks to [@WarrenFerrell](https://github.com/WarrenFerrell) for the contribution.

### Bug Fix
- [Issue #24](https://github.com/theramis/Snapper/issues/24) [PR #62](https://github.com/theramis/Snapper/pull/62) Catch more exceptions when resolving test method.

## [2.2.3] - 2020-02-15
### Bug Fix
- [Issue #53](https://github.com/theramis/Snapper/issues/53) [PR #55](https://github.com/theramis/Snapper/pull/55) Use JObjectHelper when storing snapshots. Thanks to [@ViceIce](https://github.com/ViceIce) for surfacing the issue.

## [2.2.2] - 2020-02-01
### Bug Fix
- [Issue #50](https://github.com/theramis/Snapper/issues/50) [PR #51](https://github.com/theramis/Snapper/pull/51) Fixed parsing of metadata properties so that they are treated as string by NewtonSoft. Thanks to [@ViceIce](https://github.com/ViceIce) for surfacing the issue.
- Detection of an CI environment now checks for environment variables at the `Machine`, `Process` and `User` level. Previously it only checked at the `Process` level.

## [2.2.1] - 2020-01-14
### Bug Fix
- [Issue #44](https://github.com/theramis/Snapper/issues/44) [PR #45](https://github.com/theramis/Snapper/pull/45) Fixed parsing of datetime strings so that they are treated as string by NewtonSoft. Thanks to [@plitwinski](https://github.com/plitwinski) for surfacing the issue.

## [2.2.0] - 2019-11-01
### Added
- [PR #31](https://github.com/theramis/Snapper/pull/31) `Snapper` now supports the MSTest framework. Thanks to [@tskimmett](https://github.com/tskimmett) for the contribution.

### Bug Fix
- [PR #29](https://github.com/theramis/Snapper/pull/29) Fixed a broken link to the FAQS page. Thanks to [@tomasbruckner](https://github.com/tomasbruckner) for the fix.

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

[2.3.1]: https://github.com/theramis/Snapper/compare/2.3.0...2.3.1
[2.3.0]: https://github.com/theramis/Snapper/compare/2.2.4...2.3.0
[2.2.4]: https://github.com/theramis/Snapper/compare/2.2.3...2.2.4
[2.2.3]: https://github.com/theramis/Snapper/compare/2.2.2...2.2.3
[2.2.2]: https://github.com/theramis/Snapper/compare/2.2.1...2.2.2
[2.2.1]: https://github.com/theramis/Snapper/compare/2.2.0...2.2.1
[2.2.0]: https://github.com/theramis/Snapper/compare/2.1.0...2.2.0
[2.1.0]: https://github.com/theramis/Snapper/compare/2.0.1...2.1.0
[2.0.1]: https://github.com/theramis/Snapper/compare/2.0.0...2.0.1
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
