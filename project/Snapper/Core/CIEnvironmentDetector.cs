using System;
using System.Collections.Generic;
using System.Linq;

namespace Snapper.Core
{
    internal static class CiEnvironmentDetector
    {
        /// <summary>
        ///     Based on https://github.com/watson/ci-info/blob/2012259979fc38517f8e3fc74daff714251b554d/index.js#L52-L59
        /// </summary>
        private static IEnumerable<string> CIEnvironmentVariables = new List<string>
        {
            "CI", // Travis CI, CircleCI, Cirrus CI, Gitlab CI, Appveyor, CodeShip, dsari
            "CONTINUOUS_INTEGRATION", // Travis CI, Cirrus CI
            "BUILD_NUMBER", // Jenkins, TeamCity
            "BUILD_BUILDNUMBER", // Azure DevOps
            "RUN_ID" // TaskCluster, dsari
        };

        public static bool IsCiEnv()
        {
            foreach (var envVarTarget in new[] { EnvironmentVariableTarget.Process, EnvironmentVariableTarget.Machine, EnvironmentVariableTarget.User })
            {
                var found = CIEnvironmentVariables.Any(ciEnvironmentVariable =>
                    Environment.GetEnvironmentVariable(ciEnvironmentVariable, envVarTarget) != null);
                if (found)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
