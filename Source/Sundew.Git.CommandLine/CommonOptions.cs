// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonOptions.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Git.CommandLine;

using System;
using Sundew.CommandLine;

/// <summary>Contains command line option commonly used by git.</summary>
public static class CommonOptions
{
    /// <summary>Configures the specified arguments builder.</summary>
    /// <param name="argumentsBuilder">The arguments builder.</param>
    /// <param name="quiet">The quiet.</param>
    /// <param name="setQuiet">The set quiet action.</param>
    public static void ConfigureQuiet(IArgumentsBuilder argumentsBuilder, bool quiet, Action<bool> setQuiet)
    {
        argumentsBuilder.AddSwitch(
            "q",
            "quiet",
            quiet,
            setQuiet,
            "Only print error and warning messages; all other output will be suppressed.");
    }

    /// <summary>Configures the verbose.</summary>
    /// <param name="argumentsBuilder">The arguments builder.</param>
    /// <param name="verbose">if set to <c>true</c> [verbose].</param>
    /// <param name="setVerbose">The set verbose.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "Bug! it a string with a new line.")]
    public static void ConfigureVerbose(IArgumentsBuilder argumentsBuilder, bool verbose, Action<bool> setVerbose)
    {
        argumentsBuilder.AddSwitch(
            "v",
            "verbose",
            verbose,
            setVerbose,
            @"Show unified diff between the HEAD commit and what would be committed at the bottom of the commit message template
to help the user describe the commit by reminding what changes the commit has.");
    }
}