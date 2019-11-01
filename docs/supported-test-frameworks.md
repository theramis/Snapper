---
title: Supported Test Frameworks
nav_order: 2
---

# Supported Test Frameworks

Snapper currently supports the following testing frameworks.
- [xUnit](https://xunit.net/)
- [NUnit](https://nunit.org/)
- [MSTest](https://github.com/microsoft/testfx)

## Request for a test new framework to be added
Please create a new issue in the Snapper github project mentioning the test framework you would like Snapper to support. [Link to create issue](https://github.com/theramis/Snapper/issues/new)

## How to add support for a new test framework
Contributions are always welcome and adding support for a new test framework is very easy.

To add support for a new test framework follow the following steps.

1. Create a new class which implements `ITestMethod`. See [XunitFactMethod](https://github.com/theramis/Snapper/blob/master/project/Snapper/Core/TestMethodResolver/TestMethods/XunitFactMethod.cs) for an example.
2. Update [TestMethodResolver](https://github.com/theramis/Snapper/blob/master/project/Snapper/Core/TestMethodResolver/TestMethodResolver.cs#L16) to include the newly made class.
3. Create some tests to make sure the new test framework is working in the [Snapper.TestFrameworkSupport.Tests](https://github.com/theramis/Snapper/tree/master/project/Tests/Snapper.TestFrameworkSupport.Tests) project.
4. Create a PR with the changes.
