# Snapper V2
**Bringing Jest-esque Snapshot testing to C#**

<!-- <p align="center">
    Add logo here once I find/make one
</p>
<h2 align="center">Bringing Jest-esque Snapshot testing to C#</h2> -->

[![Build Status](https://img.shields.io/appveyor/ci/theramis/snapper.svg?style=flat-square)](https://ci.appveyor.com/project/theramis/snapper)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Snapper.svg?style=flat-square)](https://www.nuget.org/packages/Snapper)
[![license](https://img.shields.io/github/license/theramis/Snapper?style=flat-square)](https://github.com/theramis/Snapper/blob/master/LICENSE)

Snapper is a [NuGet library](https://www.nuget.org/packages/Snapper) which makes snapshot testing very easy in C#. Snapshot testing can simplify testing and is super useful when paired with Golden Master testing or contract testing APIs.
It is very heavily based on [Jest Snapshot Testing framework](https://jestjs.io/docs/en/snapshot-testing).

See [https://theramis.github.io/Snapper/](https://theramis.github.io/Snapper/) for the full documentation.

## What is Snapper?

The best way to describe what Snapper does by going through an example.
Imagine you have the following test where are retrieving a user from a service.

```csharp
[Fact]
public void MyTest()
{
  var myUser = _userService.GetUser(id);
  Assert.Equal("MyUser", myUser.Username);
  Assert.Equal("email@example.com", myUser.Email);
  Assert.Equal("myhash", myUser.PasswordHash);
  ...
  ...
}

```
As you can imagine the assertion steps in the test can get very long and hard to read.
Now lets see what the test would look like if Snapper is used.
```csharp
[Fact]
public void MyTest()
{
  var myUser = _userService.GetUser(id);
  myUser.ShouldMatchSnapshot();
}

```
The test above is now asserting the `myUser` object with a snapshot stored on disk. Snapper helps you create this snapshot at the beginnging (see [Quick Start](pages/quickstart.md)).

This is the basis of snapshot testing. The idea being a baseline is first generated (in this case a json file which is our snapshot) and then everytime the test runs the output is compared to the snapshot. If the snapshot and the output from the tests don't match the test fails!

As you can see using Snapper the test is now much smaller and easier to read but that's not all. Using Snapper brings with it a lot more benefits!


## Why use Snapper?

Benefits of using Snapper/snapshot testing vs traditional assertions
- **Much easier to read** - It's quite common to have a large list of assertions which can be hard to read. Snapper makes your tests a lot shorter and easier to read!
- **Very difficult to miss properties to assert** - It's hard to validate that all properties have are being asserted using traditional assertions. By using Snapper the whole object asserted which means all properties are always asserted, so there is no chance of missing properties!
- **Captures changes to the object being asserted** - It's quite common to add new properties to our objects over time. e.g. Adding `FirstName` to the `myUser` object above. Using traditional assertions the test would still pass and it's easy to forget to update the test. Using Snapper the test would immediately fail since it's a change in the system and the developer should verify if the change was expected!
- **Much quicker to write tests** - Writing all those assertions above can be time consuming. With Snapper a json file is generated with the object which the developer can quickly verify!


## When to use Snapper?

Use cases where Snapper/snapshot testing really shines
- **Contract testing** - Testing your contract has not changed is a major part of maintaining any library/API. Snapper is excellent for this! Using Snapper any changes to the contract would immediately fail a test which lets the developer know that they might be breaking a contract they didn't expect.
- **Asserting complex objects** - Sometimes you have to assert complex objects which can be hard and time consuming to get right. Snapper makes this easy and quick.
- **Golden Master testing** - [Golden master testing](https://en.wikipedia.org/wiki/Characterization_test) is a technique where you capture the behaviour of a system. Snapper is perfect for this as you can easily assert the behaviour without the complex setup and assertions. Snapper would also fail as soon as the behaviour of the system changes

The use cases above are just some of the examples I've found where Snapper is super useful. Feel free to try them in other situation you think would be useful.

<!-- ## Snapper V1 is deprecated
After a lot of thought I've decided to deprecate Snapper V1.
Snapper V1 was my first attempt at an OSS library and some of the decisions I made very early on made it very difficult to add new features.
Snapper V2 is my second attempt at making the library easier to use and update.

Snapper V1 consisted of the following NuGet packages all of which are deprecated:
- Snapper.Core
- Snapper.Json
- Snapper.Json.Xunit
- Snapper.Json.Nunit

There is a migration guide available [here](https://theramis.github.io/Snapper/migration.html)

The changes in V2 are documented in the [Changelog](https://theramis.github.io/Snapper/changelog.html)

## Todo
- Add logo to Nuget
- Write tests for testing json store.
- Update V1 package descriptions to mention deprecated
- Use appveyor logger on all test projects -->

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="https://github.com/fgather"><img src="https://avatars3.githubusercontent.com/u/614354?v=4" width="100px;" alt=""/><br /><sub><b>Florian Gather</b></sub></a><br /><a href="https://github.com/theramis/Snapper/commits?author=fgather" title="Code">💻</a> <a href="#ideas-fgather" title="Ideas, Planning, & Feedback">🤔</a></td>
    <td align="center"><a href="https://www.linkedin.com/in/tomasbruckner/"><img src="https://avatars2.githubusercontent.com/u/7334618?v=4" width="100px;" alt=""/><br /><sub><b>Tomas Bruckner</b></sub></a><br /><a href="https://github.com/theramis/Snapper/commits?author=tomasbruckner" title="Code">💻</a></td>
    <td align="center"><a href="https://visualon.de"><img src="https://avatars1.githubusercontent.com/u/1798109?v=4" width="100px;" alt=""/><br /><sub><b>Michael Kriese</b></sub></a><br /><a href="https://github.com/theramis/Snapper/commits?author=ViceIce" title="Code">💻</a> <a href="#ideas-ViceIce" title="Ideas, Planning, & Feedback">🤔</a> <a href="https://github.com/theramis/Snapper/issues?q=author%3AViceIce" title="Bug reports">🐛</a></td>
    <td align="center"><a href="http://cognitoforms.com"><img src="https://avatars0.githubusercontent.com/u/4603206?v=4" width="100px;" alt=""/><br /><sub><b>Taylor Kimmett</b></sub></a><br /><a href="https://github.com/theramis/Snapper/commits?author=tskimmett" title="Code">💻</a></td>
    <td align="center"><a href="https://github.com/PatrickLehnerXI"><img src="https://avatars1.githubusercontent.com/u/19566691?v=4" width="100px;" alt=""/><br /><sub><b>Patrick Lehner</b></sub></a><br /><a href="https://github.com/theramis/Snapper/issues?q=author%3APatrickLehnerXI" title="Bug reports">🐛</a></td>
    <td align="center"><a href="https://github.com/plitwinski"><img src="https://avatars3.githubusercontent.com/u/25408297?v=4" width="100px;" alt=""/><br /><sub><b>Piotr Litwinski</b></sub></a><br /><a href="https://github.com/theramis/Snapper/issues?q=author%3Aplitwinski" title="Bug reports">🐛</a></td>
    <td align="center"><a href="https://github.com/WarrenFerrell"><img src="https://avatars0.githubusercontent.com/u/8977001?v=4" width="100px;" alt=""/><br /><sub><b>Warren Ferrell</b></sub></a><br /><a href="https://github.com/theramis/Snapper/commits?author=WarrenFerrell" title="Code">💻</a></td>
  </tr>
  <tr>
    <td align="center"><a href="https://github.com/lilasquared"><img src="https://avatars3.githubusercontent.com/u/3036779?v=4" width="100px;" alt=""/><br /><sub><b>Aaron Roberts</b></sub></a><br /><a href="https://github.com/theramis/Snapper/commits?author=lilasquared" title="Code">💻</a> <a href="#ideas-lilasquared" title="Ideas, Planning, & Feedback">🤔</a></td>
  </tr>
</table>

<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->
<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!
