# Changelog
All notable changes to the Snapper project will be documented in this file.

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

[1.3.0]: https://github.com/theramis/Snapper/compare/1.2.0...1.3.0
[1.2.0]: https://github.com/theramis/Snapper/compare/1.1.1...1.2.0
[1.1.1]: https://github.com/theramis/Snapper/compare/1.1.0...1.1.1
[1.1.0]: https://github.com/theramis/Snapper/compare/1.0.1...1.1.0
[1.0.1]: https://github.com/theramis/Snapper/compare/1.0.0...1.0.1
[1.0.0]: https://github.com/theramis/Snapper/tree/1.0.0
