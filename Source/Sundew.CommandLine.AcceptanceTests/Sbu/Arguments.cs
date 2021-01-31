// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Arguments.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Sbu
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public class Arguments : IArguments
    {
        private const string VersionGroupName = "Version";
        private const string LocalSundewName = "Local-Sundew";
        private static readonly Regex PackageIdAndVersionRegex = new Regex(@"(?: |\.)(?<Version>(?:\d\.\d\.\d).*)");
        private readonly List<PackageId> packageIds;
        private readonly List<string> projects;
        private bool useLocalSource;

        public Arguments()
        : this(new List<PackageId> { new("*") }, new List<string> { "*" })
        {
        }

        public Arguments(List<PackageId> packageIds, List<string> projects, string? source = null, Version? nuGetVersion = null, string? rootDirectory = null, bool allowPrerelease = false, bool verbose = false, bool useLocalSource = false)
        {
            this.packageIds = packageIds;
            this.projects = projects;
            this.Source = source;
            this.NuGetVersion = nuGetVersion;
            this.RootDirectory = rootDirectory;
            this.AllowPrerelease = allowPrerelease;
            this.Verbose = verbose;
            this.UseLocalSource = useLocalSource;
        }

        public IReadOnlyList<PackageId> PackageIds => this.packageIds;

        public IReadOnlyList<string> Projects => this.projects;

        public string? Source { get; private set; }

        public Version? NuGetVersion { get; private set; }

        public string? RootDirectory { get; private set; }

        public bool AllowPrerelease { get; private set; }

        public bool Verbose { get; private set; }

        public bool UseLocalSource
        {
            get => this.useLocalSource;
            private set
            {
                this.useLocalSource = value;
                if (this.UseLocalSource)
                {
                    this.Source = LocalSundewName;
                }
            }
        }

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddOptionalList("id", "package-ids", this.packageIds, this.Serialize, this.Deserialize, @$"The package(s) to update. (* Wildcards supported){Environment.NewLine}Format: Id[.Version] or ""Id[ Version]"" (Pinning version is optional)");
            argumentsBuilder.AddOptionalList("p", "projects", this.projects, "The project(s) to update (* Wildcards supported)");
            argumentsBuilder.AddOptional("s", "source", () => this.Source, s => this.Source = s, "The source or source name to search for packages (All supported)", defaultValueText: "NuGet.config: defaultPushSource");
            argumentsBuilder.AddOptional(null, "version", () => this.NuGetVersion?.ToString(), s => this.NuGetVersion = Version.Parse(s), "Pins the NuGet package version.", defaultValueText: "Latest if not pinned");
            argumentsBuilder.AddOptional("d", "root-directory", () => this.RootDirectory, s => this.RootDirectory = s, "The directory to search to projects", true, defaultValueText: "Current directory");
            argumentsBuilder.AddSwitch("pr", "prerelease", this.AllowPrerelease, b => this.AllowPrerelease = b, "Allow updating to latest prerelease version");
            argumentsBuilder.AddSwitch("v", "verbose", this.Verbose, b => this.Verbose = b, "Verbose");
            argumentsBuilder.AddSwitch("l", "local", this.UseLocalSource, b => this.UseLocalSource = b, $@"Forces the source to ""{LocalSundewName}""");
        }

        private string Serialize(PackageId id, CultureInfo cultureInfo)
        {
            if (id.Version != null)
            {
                return $"{id.Id}.{id.Version}";
            }

            return id.Id;
        }

        private PackageId Deserialize(string id, CultureInfo cultureInfo)
        {
            var match = PackageIdAndVersionRegex.Match(id);
            if (match.Success)
            {
                var versionGroup = match.Groups[VersionGroupName];
                if (versionGroup.Success)
                {
                    return new PackageId(id.Substring(0, versionGroup.Index - 1), Version.Parse(versionGroup.Value));
                }
            }

            return new PackageId(id);
        }
    }
}
