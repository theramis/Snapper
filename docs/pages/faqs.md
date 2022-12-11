# Frequently asked questions

### Why am I getting a `SupportedTestMethodNotFoundException`?
```
Snapper.Exceptions.SupportedTestMethodNotFoundException : A supported test method was not found.
Make sure you are using Snapper inside a supported test framework. See https://theramis.github.io/Snapper/#/pages/faqs for more info.
```
If you got an exception when running tests with Snapper similiar to the one above there are two reasons why this could be happening.

The first is that the test framework being used is not supported. See [Supported Test Frameworks](supported-test-frameworks.md) page.

The second reason this can happen is when the testing are running in a release build configuration.
Snapper uses the [StackTrace](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.stacktrace?view=netstandard-2.0) class to determine which test method it was called from. In a Release build the compiler optimises the code which can cause some issues with how Snapper determines the test method. The compiler in this case is inlining the method which makes the method invisible to the `StackTrace` class. See [link](https://stackoverflow.com/questions/3924995/what-is-method-inlining) for a better explanation.

There are currently three solutions for this issue.
1. Set the following attribute on the test method `[MethodImpl(MethodImplOptions.NoInlining)]` as seen [here](https://github.com/theramis/Snapper/blob/bd6fa1e73f1c30f4b2bdda52ddf7bcd3029cacbc/project/Tests/Snapper.Tests/SnapperSnapshotsPerMethodTests.cs#L12).
2. Disable optimisation of code for your release builds of your test project. You can do this by adding this into your projects csproj file.
```xml
    <PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
        <Optimize>false</Optimize>
    </PropertyGroup>
```
3. Create a PDB symbol file with the full debug setting.
```xml
    <PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
        <DebugType>full</DebugType>
    </PropertyGroup>
```

For more information see this issue: [theramis/snapper#16](https://github.com/theramis/Snapper/issues/16)

If none of the solutions above work its possible you have encountered a usecase where Snapper is unable to determine the test. There has been one use reported use case like this before. See https://github.com/theramis/Snapper/issues/30.
In this case the recommended solution is to use [Custom Snapshot Settings](/pages/snapper/custom_snapshot_settings?id=customising-snapshot-file-location) to create snapshots.

### Why am I getting a `UnableToDetermineTestFilePathException`?
```
Snapper.Exceptions.UnableToDetermineTestFilePathException : Unable to determine the file path of the test method.
Make sure optimisation of the test project is disabled. See https://theramis.github.io/Snapper/#/pages/faqs for more info.
```

This can happen when the PDB files are not generated for the test project. Try setting the `<Optimize>false</Optimize>` or `<DebugType>full</DebugType>` settings in your projects csproj file. See above for more details on how to set these.
