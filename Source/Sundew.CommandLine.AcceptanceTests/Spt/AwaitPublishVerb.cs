// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AwaitPublishVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Spt
{
    using System;
    using System.Text.RegularExpressions;
    using NuGet.Versioning;
    using Sundew.CommandLine;

    public class AwaitPublishVerb : IVerb
    {
        internal static readonly Regex PackageIdAndVersionRegex = new(@"(?: |\.)(?<Version>(?:\d+\.\d+(?<Patch>\.\d+)?).*)");

        public AwaitPublishVerb()
            : this(default!)
        {
        }

        public AwaitPublishVerb(PackageIdAndVersion packageIdAndVersion, string? source = null, string? rootDirectory = null, int timeoutInSeconds = 300, bool verbose = false)
        {
            this.PackageIdAndVersion = packageIdAndVersion;
            this.Source = source;
            this.RootDirectory = rootDirectory;
            this.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            this.Verbose = verbose;
        }

        public IVerb? NextVerb { get; } = null;

        public string Name { get; } = "await";

        public string? ShortName { get; } = "a";

        public string HelpText { get; } = "Awaits the specified package to be published";

        public PackageIdAndVersion PackageIdAndVersion { get; private set; }

        public string? Source { get; private set; }

        public string? RootDirectory { get; private set; }

        public TimeSpan Timeout { get; private set; }

        public bool Verbose { get; private set; }

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddOptional("s", "source", () => this.Source, s => this.Source = s, @"The source or source name to await publish", defaultValueText: "NuGet.config: defaultPushSource");
            CommonOptions.AddRootDirectory(argumentsBuilder, () => this.RootDirectory, s => this.RootDirectory = s);
            argumentsBuilder.AddOptional(
                "t",
                "timeout",
                (ci) => this.Timeout.TotalSeconds.ToString(ci),
                (s, ci) => this.Timeout = TimeSpan.FromSeconds(double.Parse(s, ci)),
                @"The wait timeout in seconds");
            argumentsBuilder.AddRequiredValue("package-id", this.SerializePackageId, this.DeserializePackageId, $"Specifies the package id and optionally the version{Environment.NewLine}Format: <PackageId>[.<Version>].{Environment.NewLine}If the version is not provided, it must be specified by the version value");
            argumentsBuilder.AddOptionalValue("version", this.SerializeVersion, this.DeserializeVersion, "Specifies the NuGet Package version");
            CommonOptions.AddVerbose(argumentsBuilder, this.Verbose, b => this.Verbose = b);
        }

        private string SerializeVersion()
        {
            if (this.PackageIdAndVersion == null)
            {
                return string.Empty;
            }

            return this.PackageIdAndVersion.NuGetVersion.ToFullString();
        }

        private string SerializePackageId()
        {
            if (this.PackageIdAndVersion == null)
            {
                return string.Empty;
            }

            if (this.PackageIdAndVersion.NuGetVersion == null)
            {
                return $"{this.PackageIdAndVersion.Id}";
            }

            return $"{this.PackageIdAndVersion.Id}.{this.PackageIdAndVersion.NuGetVersion}";
        }

        private void DeserializeVersion(string version)
        {
            this.PackageIdAndVersion = this.PackageIdAndVersion with { NuGetVersion = NuGetVersion.Parse(version) };
        }

        private void DeserializePackageId(string id)
        {
            var match = PackageIdAndVersionRegex.Match(id);
            if (match.Success)
            {
                var versionGroup = match.Groups[CommonOptions.VersionGroupName];
                if (versionGroup.Success)
                {
                    this.PackageIdAndVersion = new PackageIdAndVersion(id.Substring(0, versionGroup.Index - 1), NuGetVersion.Parse(versionGroup.Value));
                    return;
                }
            }

            this.PackageIdAndVersion = new PackageIdAndVersion(id, NuGetVersion.Parse("0.0.0"));
        }
    }
}