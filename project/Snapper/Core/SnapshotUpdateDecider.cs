using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Snapper.Attributes;
using Snapper.Core.TestMethodResolver;

namespace Snapper.Core
{
    internal class SnapshotUpdateDecider : ISnapshotUpdateDecider
    {
        private readonly ITestMethodResolver _testMethodResolver;
        private const string UpdateSnapshotEnvironmentVariableName = "UpdateSnapshots";
        private readonly string _envVarName;

        /// <summary>
        ///     Based on https://github.com/watson/ci-info/blob/2012259979fc38517f8e3fc74daff714251b554d/index.js#L52-L59
        /// </summary>
        private readonly IEnumerable<string> _ciEnvironmentVariables = new List<string>
        {
            "CI", // Travis CI, CircleCI, Cirrus CI, Gitlab CI, Appveyor, CodeShip, dsari
            "CONTINUOUS_INTEGRATION", // Travis CI, Cirrus CI
            "BUILD_NUMBER", // Jenkins, TeamCity
            "BUILD_BUILDNUMBER", // Azure DevOps
            "RUN_ID" // TaskCluster, dsari
        };

        public SnapshotUpdateDecider(ITestMethodResolver testMethodResolver,
            string envVarName = UpdateSnapshotEnvironmentVariableName)
        {
            _testMethodResolver = testMethodResolver;
            _envVarName = envVarName;
        }

        public bool ShouldUpdateSnapshot()
            => ShouldUpdateSnapshotBasedOnEnvironmentVariable()
                   || ShouldUpdateSnapshotBasedOnAttribute();

        private bool ShouldUpdateSnapshotBasedOnEnvironmentVariable()
        {
            var env = Environment.GetEnvironmentVariable(_envVarName);
            if (env == null)
                return false;
            return bool.TryParse(env, out var value) && value;
        }

        private bool ShouldUpdateSnapshotBasedOnAttribute()
        {
            var method = _testMethodResolver.ResolveTestMethod().BaseMethod;

            var customAttributeProviders = new List<ICustomAttributeProvider>
            {
                method, // check method
                method?.DeclaringType, // check class
                method?.DeclaringType?.Assembly // check assembly
            };

            foreach (var customAttributeProvider in customAttributeProviders)
            {
                if (TryGetUpdateSnapshotsAttribute(customAttributeProvider, out var att))
                {
                    return !(att.IgnoreIfCi && IsCiEnv());
                }
            }

            return false;
        }

        private bool IsCiEnv()
        {
            foreach (var envVarTarget in new[] { EnvironmentVariableTarget.Process, EnvironmentVariableTarget.Machine, EnvironmentVariableTarget.User})
            {
                var found = _ciEnvironmentVariables.Any(ciEnvironmentVariable =>
                        Environment.GetEnvironmentVariable(ciEnvironmentVariable, envVarTarget) != null);
                if (found)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryGetUpdateSnapshotsAttribute(ICustomAttributeProvider member, out UpdateSnapshotsAttribute attribute)
        {
            var attributes = member?.GetCustomAttributes(typeof(UpdateSnapshotsAttribute), true);

            attribute = attributes?.FirstOrDefault() as UpdateSnapshotsAttribute;
            return attributes?.Any() ?? false;
        }
    }
}
