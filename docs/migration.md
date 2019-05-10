---
title: Migration
nav_order: 6
---

# How to migrate from V1 to V2

Migrating from V1 to V2 should be a relatively quick job.

1. Remove the the following NuGet packages for Snapper V1 (You probably won't have all of them in your project)
- `Snapper.Core`
- `Snapper.Json`
- `Snapper.Json.Xunit`
- `Snapper.Json.Nunit`

2. Install `Snapper` or the `Snapper.Nunit` NuGet package into your project.

3. Migrate the following calls from V1 to V2 in the code base.

    | V1 Call | V2 Call |
    | ------- | ------- |
    |`XUnitSnapper.MatchSnapshot(object)` | `object.ShouldMatchSnapshot()` |
    |`XUnitSnapper.MatchSnapshot(object, 'name')` | `object.ShouldMatchChildSnapshot('name')` |
    |`Assert.That(actual, Is.EqualToSnapshot("name"))` | `Assert.That(actual, Is.EqualToChildSnapshot("name"));` |

4. You will need to regenerate all of your snapshots again. Delete all of the snapshots in the project. Use the `[assembly: UpdateSnapshots]` attribute and run all of the tests

5. Verify that all the newly created snapshots are correct.