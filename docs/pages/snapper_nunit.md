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
        Assert.That(obj, Is.EqualToSnapshot());
    }

    [Test]
    public void MySecondTest(){
        var obj = new {
            Key = "value"
        };
        // Best to use with theory tests
        // See https://theramis.github.io/Snapper/snapper/basics.html#child-snapshots for more information about child snapshots
        Assert.That(obj, Is.EqualToChildSnapshot("NamedSnapshot"));
    }
}
```