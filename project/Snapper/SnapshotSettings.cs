using System;
using Newtonsoft.Json;
using Snapper.Attributes;

namespace Snapper;

/// <summary>
///     SnapshotSettings allows you to have greater control over your snapshots.
///     See <a href="https://theramis.github.io/Snapper/#/pages/snapper/custom_snapshot_settings">the docs</a>
///     for more information on how to use SnapshotSettings.
/// </summary>
public class SnapshotSettings
{
    /// <summary>
    /// Use this method to configure global serialiser settings for how snapshots are serialised/de-serialised
    /// This method is called first before settings are configured by <see cref="SnapshotSerializerSettings"/>
    /// </summary>
    public static Action<JsonSerializerSettings>? GlobalSnapshotSerialiserSettings;

    internal bool? ShouldStoreSnapshotsPerClass;
    internal string? Directory;
    internal string? FileName;
    internal string? ClassName;
    internal string? TestName;
    internal Action<JsonSerializerSettings>? SnapshotSerialiserSettings;

    public static SnapshotSettings New() => new();

    /// <summary>
    /// Set the directory where the snapshot should be stored.
    /// </summary>
    /// <param name="directory">The full path to the directory.</param>
    public SnapshotSettings SnapshotDirectory(string directory)
    {
        Directory = directory;
        return this;
    }

    /// <summary>
    /// Set the filename of the snapshot.
    /// </summary>
    /// <param name="filename">The filename without the file extension</param>
    public SnapshotSettings SnapshotFileName(string filename)
    {
        FileName = filename;
        return this;
    }

    /// <summary>
    /// Set whether the snapshot should be stored per class or not.
    /// This overrides the <see cref="StoreSnapshotsPerClassAttribute"/> attribute.
    /// </summary>
    /// <param name="storeSnapshotsPerClass">Whether to store snapshots per class or not</param>
    public SnapshotSettings StoreSnapshotsPerClass(bool storeSnapshotsPerClass)
    {
        ShouldStoreSnapshotsPerClass = storeSnapshotsPerClass;
        return this;
    }

    /// <summary>
    /// Set the class name, this can be useful when Snapper cannot detect the test class automatically.
    /// </summary>
    /// <param name="className">The classname to use</param>
    public SnapshotSettings SnapshotClassName(string className)
    {
        ClassName = className;
        return this;
    }

    /// <summary>
    /// Set the snapshot test name, this can be useful when Snapper cannot detect the test method automatically.
    /// </summary>
    /// <param name="snapshotTestName">The snapshot test name to use</param>
    public SnapshotSettings SnapshotTestName(string snapshotTestName)
    {
        TestName = snapshotTestName;
        return this;
    }

    /// <summary>
    /// Use this method to configure custom serialiser settings for how snapshots are serialised/de-serialised
    /// This method is called AFTER settings are configured by <see cref="GlobalSnapshotSerialiserSettings"/>
    /// </summary>
    /// <param name="func">Method to configure serialiser settings</param>
    public SnapshotSettings SnapshotSerializerSettings(Action<JsonSerializerSettings> func)
    {
        SnapshotSerialiserSettings = func;
        return this;
    }
}
