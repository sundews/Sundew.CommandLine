﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneLocalSourceVerb.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Spt;

using Sundew.CommandLine;

public class PruneLocalSourceVerb : IVerb
{
    public PruneLocalSourceVerb()
    {
    }

    public PruneLocalSourceVerb(IPruneModeVerb purgeModeVerb)
    {
        this.NextVerb = purgeModeVerb;
    }

    public IVerb? NextVerb { get; }

    public string Name { get; } = "prune";

    public string? ShortName { get; } = "p";

    public string HelpText { get; } = "Prunes the matching packages for a local source";

    public void Configure(IArgumentsBuilder argumentsBuilder)
    {
    }
}