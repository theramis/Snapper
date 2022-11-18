using System;
using System.Collections.Generic;
using System.Linq;

namespace Snapper.Core
{
    internal static class CiEnvironmentDetector
    {
        /// <summary>
        ///     Based on https://github.com/watson/ci-info/blob/decd4f7afedaef0d9875a41b8049d207798db4b1/index.js#L56-L68
        /// </summary>
        private static readonly IEnumerable<string> CIEnvironmentVariables = new List<string>
        {
            "BUILD_ID", // Jenkins, Cloudbees
            "BUILD_NUMBER", // Jenkins, TeamCity
            "CI", // Travis CI, CircleCI, Cirrus CI, Gitlab CI, Appveyor, CodeShip, dsari
            "CI_APP_ID", // Appflow
            "CI_BUILD_ID", // Appflow
            "CI_BUILD_NUMBER", // Appflow
            "CI_NAME", // Codeship and others
            "CONTINUOUS_INTEGRATION", // Travis CI, Cirrus CI
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
