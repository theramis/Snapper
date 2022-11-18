using System;
using Newtonsoft.Json;

namespace Snapper;

// TODO Add docs in readme and here
public class SnapshotSettings
{
    public static Action<JsonSerializerSettings>? GlobalSnapshotSerialiserSettings;

    internal bool? ShouldStoreSnapshotsPerClass;
    internal string? Directory;
    internal string? FileName;
    internal string? ClassName;
    internal string? TestName;
    internal Action<JsonSerializerSettings>? SnapshotSerialiserSettings;

    public static SnapshotSettings New() => new();

    public SnapshotSettings SnapshotDirectory(string directory)
    {
        Directory = directory;
        return this;
    }

    public SnapshotSettings SnapshotFileName(string filename)
    {
        FileName = filename;
        return this;
    }

    public SnapshotSettings StoreSnapshotsPerClass(bool storeSnapshotsPerClass)
    {
        ShouldStoreSnapshotsPerClass = storeSnapshotsPerClass;
        return this;
    }

    public SnapshotSettings SnapshotClassName(string className)
    {
        ClassName = className;
        return this;
    }

    public SnapshotSettings SnapshotTestName(string snapshotTestName)
    {
        TestName = snapshotTestName;
        return this;
    }

    public SnapshotSettings SnapshotSerializerSettings(Action<JsonSerializerSettings> func)
    {
        SnapshotSerialiserSettings = func;
        return this;
    }
}
