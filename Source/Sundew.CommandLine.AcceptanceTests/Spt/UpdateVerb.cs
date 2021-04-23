// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Spt
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Sundew.CommandLine;

    public class UpdateVerb : IVerb
    {
        private const string Star = "*";
        private const string VersionRegexText = @"(?<Version>[\d\.\*]+(?:(?:-(?<Prerelease>[^\.\s]+))|$))";
        private static readonly Regex PackageIdAndVersionRegex = new(@$"(?: |\.){VersionRegexText}");
        private static readonly Regex VersionRegex = new(VersionRegexText);
        private readonly List<PackageId> packageIds;
        private readonly List<string> projects;
        private bool useLocalSource;

        public UpdateVerb()
        : this(new List<PackageId> { new(Star) }, new List<string> { Star })
        {
        }

        public UpdateVerb(List<PackageId> packageIds, List<string> projects, string? source = null, string? versionPattern = null, string? rootDirectory = null, bool allowPrerelease = false, bool verbose = false, bool useLocalSource = false, bool skipRestore = false)
        {
            this.packageIds = packageIds;
            this.projects = projects;
            this.Source = source;
            this.VersionPattern = versionPattern;
            this.RootDirectory = rootDirectory;
            this.AllowPrerelease = allowPrerelease;
            this.Verbose = verbose;
            this.UseLocalSource = useLocalSource;
            this.SkipRestore = skipRestore;
        }

        public IVerb? NextVerb { get; }

        public string Name { get; } = "update";

        public string? ShortName { get; } = "u";

        public string HelpText { get; } = "Update package references";

        public IReadOnlyList<PackageId> PackageIds => this.packageIds;

        public IReadOnlyList<string> Projects => this.projects;

        public string? Source { get; private set; }

        public string? VersionPattern { get; private set; }

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
                    this.Source = CommonOptions.LocalSundewName;
                }
            }
        }

        public bool SkipRestore { get; private set; }

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddOptionalList("id", "package-ids", this.packageIds, this.SerializePackageId, this.DeserializePackageId, @$"The package(s) to update. (* Wildcards supported){Environment.NewLine}Format: Id[.Version] or ""Id[ Version]"" (Pinning version is optional)");
            argumentsBuilder.AddOptionalList("p", "projects", this.projects, "The project(s) to update (* Wildcards supported)");
            argumentsBuilder.AddOptional("s", "source", () => this.Source, s => this.Source = s, @"The source or source name to search for packages (""All"" supported)", defaultValueText: "NuGet.config: defaultPushSource");
            argumentsBuilder.AddOptional(null, "version", () => this.VersionPattern, s => this.VersionPattern = this.DeserializeVersion(s), "The NuGet package version (* Wildcards supported).", defaultValueText: "Latest version");
            CommonOptions.AddRootDirectory(argumentsBuilder, () => this.RootDirectory, s => this.RootDirectory = s);
            argumentsBuilder.AddSwitch("pr", "prerelease", this.AllowPrerelease, b => this.AllowPrerelease = b, "Allow updating to latest prerelease version");
            CommonOptions.AddVerbose(argumentsBuilder, this.Verbose, b => this.Verbose = b);
            argumentsBuilder.AddSwitch("l", "local", this.UseLocalSource, b => this.UseLocalSource = b, $@"Forces the source to ""{CommonOptions.LocalSundewName}""");
            argumentsBuilder.AddSwitch("sr", "skip-restore", this.SkipRestore, b => this.SkipRestore = b, "Skips a dotnet restore command after package update.");
        }

        private string? DeserializeVersion(string pinnedNuGetVersion)
        {
            var match = VersionRegex.Match(pinnedNuGetVersion);
            if (match.Success)
            {
                return match.Value;
            }

            throw new ArgumentException($"Invalid version: {pinnedNuGetVersion}", nameof(pinnedNuGetVersion));
        }

        private string SerializePackageId(PackageId id, CultureInfo cultureInfo)
        {
            if (id.VersionPattern != null)
            {
                return $"{id.Id}.{id.VersionPattern}";
            }

            return id.Id;
        }

        private PackageId DeserializePackageId(string id, CultureInfo cultureInfo)
        {
            var match = PackageIdAndVersionRegex.Match(id);
            if (match.Success)
            {
                var versionGroup = match.Groups[CommonOptions.VersionGroupName];
                if (versionGroup.Success)
                {
                    return new PackageId(id.Substring(0, versionGroup.Index - 1), versionGroup.Value);
                }
            }

            return new PackageId(id);
        }
    }
}