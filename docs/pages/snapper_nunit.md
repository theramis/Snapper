# Snapper.Nunit

The `Snapper.Nunit` NuGet package extends the Nunit constraint to provide a deeper integration with the NUnit test framework.

The following code snippet shows the constaints which have been added by Snapper.
```csharp
public class MyTestClass
{
    [Test]
    public void MyFirstTest(){
        var obj = new {
            Key = "value"
        };
        Assert.That(obj, Matches.Snapshot());
    }

    [Test]
    public void MySecondTest(){
        var obj = new {
            Key = "value"
        };
        // Best to use with theory tests
        // See https://theramis.github.io/Snapper/#/pages/snapper/basics?id=child-snapshots for more information about child snapshots
        Assert.That(obj, Matches.ChildSnapshot("NamedSnapshot"));
    }

    [Test]
    public void MyThirdTest(){
        var obj = new {
            Key = "value"
        };
        // See https://theramis.github.io/Snapper/#/pages/snapper/advanced_snapshot_control for more information about `SnapshotId`
        Assert.That(obj, Matches.Snapshot(new SnapshotId("dir", "class", "method", null, false)));
    }
}
```