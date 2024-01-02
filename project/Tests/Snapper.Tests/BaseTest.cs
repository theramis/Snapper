using System;
using System.Collections.Generic;

namespace Snapper.Tests;

// All tests should use this class as it will reset all environment variables
// before a test runs
public abstract class BaseTest
{
    private readonly List<string> _envVarsToClear = new() { "CI", "UpdateSnapshots" };

    protected BaseTest()
    {
        _envVarsToClear.ForEach(e =>
                Environment.SetEnvironmentVariable(e, null, EnvironmentVariableTarget.Process));
    }
}
