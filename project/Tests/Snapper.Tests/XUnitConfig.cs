using Xunit;

// Tests use environment variables and having them run in parallel cause issues
[assembly: CollectionBehavior(DisableTestParallelization = true)]
