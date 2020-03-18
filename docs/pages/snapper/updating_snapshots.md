# Updating Snapshots

Creating/Updating snapshots manually can be a time consuming and difficult task. Snapper makes this task easy.

There are two ways you can ask Snapper to create/update snapshots.

1. Environment variable
2. Update snapshot attribute (recommended method)

## Environment variable
Snapper checks for an environment variable called `UpdateSnapshots`. If the value for the environment variable is set to `true` it will update snapshots for all tests that are run.

## Update snapshot attribute
Snapper will check for a `[UpdateSnapshots]` attribute when running tests. If it finds that a test method/class/assembly has the attribute it will update snapshots for all tests that are run.

By default when the attribute is used, Snapper will try detect whether the tests are running in a CI environment. If a CI environment is detected then the presence of the `[UpdateSnapshots]` will be ignored.
This can be disabled by setting the `ignoreIfCi` flag to false on the attribute. e.g. `[UpdateSnapshots(false)]`