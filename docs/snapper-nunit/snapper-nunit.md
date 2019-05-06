---
title: Snapper.Nunit
nav_order: 4
---

# Snapper.Nunit

# Basics

The Snapper.Nunit NuGet package extends the NUnit constraint to provide a deeper integration with the NUnit test framework.

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
        Assert.That(obj, Is.EqualToSnapshot("NamedSnapshot"));
    }
}
```