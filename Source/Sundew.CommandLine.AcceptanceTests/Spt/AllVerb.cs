// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Spt;

using System.Collections.Generic;
using Sundew.CommandLine;

public class AllVerb : IVerb
{
    private readonly List<string> packageIds;

    public AllVerb()
        : this(new List<string> { "*" }, CommonOptions.LocalSundewName)
    {
    }

    public AllVerb(List<string> packageIds, string source, bool verbose = false)
    {
        this.packageIds = packageIds;
        this.Source = source;
        this.Verbose = verbose;
    }

    public string Source { get; private set; }

    public IReadOnlyList<string> PackageIds => this.packageIds;

    public bool Verbose { get; private set; }

    public IVerb? NextVerb { get; } = default;

    public string Name { get; } = "all";

    public string? ShortName => default;

    public string HelpText { get; } = "Prune the specified local source for all packages";

    public void Configure(IArgumentsBuilder argumentsBuilder)
    {
        argumentsBuilder.AddOptionalList("p", "package-ids", this.packageIds, "The packages to prune (* Wildcards supported)");
        argumentsBuilder.AddOptional("s", "source", () => this.Source, s => this.Source = s, @"Local source or source name to search for packages");
        CommonOptions.AddVerbose(argumentsBuilder, this.Verbose, b => this.Verbose = b);
    }
}