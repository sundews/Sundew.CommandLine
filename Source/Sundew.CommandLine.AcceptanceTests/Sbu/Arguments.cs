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

    public class Arguments : IArguments
    {
        private readonly List<PackageId> packageIds;
        private readonly List<string> projects;
        private string? source;
        private Version? version;
        private string? rootDirectory;
        private bool allowPrerelease;

        public Arguments()
        : this(new List<PackageId>(), new List<string>())
        {
        }

        public Arguments(List<PackageId> packageIds, List<string> projects, string? source = null, Version? version = null, string? rootDirectory = null, bool allowPrerelease = false)
        {
            this.packageIds = packageIds;
            this.projects = projects;
            this.source = source;
            this.version = version;
            this.rootDirectory = rootDirectory;
            this.allowPrerelease = allowPrerelease;
        }

        public IReadOnlyList<PackageId> PackageIds => this.packageIds;

        public IReadOnlyList<string> Projects => this.projects;

        public string? Source => this.source;

        public Version? Version => this.version;

        public string? RootDictionary => this.rootDirectory;

        public bool AllowPrerelease => this.allowPrerelease;

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddOptionalList("id", "package-ids", this.packageIds, this.Serialize, this.Deserialize, @"The package(s) to update. ""PackageId[ Version]""", true);
            argumentsBuilder.AddOptionalList("pn", "projects", this.projects, "The project(s) to update");
            argumentsBuilder.AddOptional("s", "source", () => this.source, s => this.source = s, "The source or source name to search for packages");
            argumentsBuilder.AddOptional("v", "version", () => this.version?.ToString(), s => this.version = Version.Parse(s), "The NuGet version (Single package only)");
            argumentsBuilder.AddOptional("d", "root-directory", () => this.rootDirectory, s => this.rootDirectory = s, "The directory to search to projects", true);
            argumentsBuilder.AddSwitch("pr", "prerelease", this.allowPrerelease, b => this.allowPrerelease = b, "Allow updating to prerelease versions");
        }

        private string Serialize(PackageId id, CultureInfo cultureInfo)
        {
            if (id.Version != null)
            {
                return $"{id.Id} {id.Version}";
            }

            return id.Id;
        }

        private PackageId Deserialize(string id, CultureInfo cultureInfo)
        {
            var idAndVersion = id.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            switch (idAndVersion.Length)
            {
                case 1:
                    return new PackageId(idAndVersion[0], default);
                case 2:
                    return new PackageId(idAndVersion[0], Version.Parse(idAndVersion[1]));
            }

            throw new ArgumentException($"Could not parse id and version from {id}. The format should be: PackageId[ Version]", nameof(id));
        }
    }
}
