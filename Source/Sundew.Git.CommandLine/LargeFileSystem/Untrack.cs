// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Untrack.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Git.CommandLine.LargeFileSystem;

using Sundew.CommandLine;

/// <summary>Stop tracking the given path(s) through Git LFS.</summary>
/// <seealso cref="Sundew.CommandLine.IVerb" />
public class Untrack : IVerb
{
    /// <summary>Initializes a new instance of the <see cref="Untrack"/> class.</summary>
    public Untrack()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="Untrack"/> class.</summary>
    /// <param name="pattern">The pattern.</param>
    public Untrack(string? pattern)
    {
        this.Pattern = pattern;
    }

    /// <summary>Gets the next verb.</summary>
    /// <value>The next verb.</value>
    public IVerb? NextVerb => null;

    /// <summary>Gets the pattern.</summary>
    /// <value>The pattern.</value>
    public string? Pattern { get; private set; }

    /// <summary>Gets the help text.</summary>
    /// <value>The help text.</value>
    public string HelpText { get; } = "Stop tracking the given patterns(s) through Git LFS.";

    /// <summary>Gets the name.</summary>
    /// <value>The name.</value>
    public string Name { get; } = "untrack";

    /// <summary>
    /// Gets the short name.
    /// </summary>
    /// <value>
    /// The short name.
    /// </value>
    public string? ShortName { get; } = null;

    /// <summary>Configures the specified arguments builder.</summary>
    /// <param name="argumentsBuilder">The arguments builder.</param>
    public void Configure(IArgumentsBuilder argumentsBuilder)
    {
        argumentsBuilder.AddOptionalValue(
            "pattern",
            () => this.Pattern,
            value => this.Pattern = value,
            "The <pattern> argument is removed from .gitattributes.");
    }
}