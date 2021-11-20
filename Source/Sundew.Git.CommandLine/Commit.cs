// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Commit.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Git.CommandLine;

using Sundew.CommandLine;

/// <summary>Commit verb.</summary>
/// <seealso cref="Sundew.CommandLine.IVerb" />
public class Commit : IVerb
{
    /// <summary>Initializes a new instance of the <see cref="Commit"/> class.</summary>
    public Commit()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="Commit"/> class.</summary>
    /// <param name="message">The message.</param>
    public Commit(string? message)
    {
        this.Message = message;
    }

    /// <summary>Gets the next verb.</summary>
    /// <value>The next verb.</value>
    public IVerb? NextVerb => null;

    /// <summary>Gets the message.</summary>
    /// <value>The message.</value>
    public string? Message { get; private set; }

    /// <summary>Gets or sets a value indicating whether this command is verbose.</summary>
    /// <value>
    ///   <c>true</c> if verbose; otherwise, <c>false</c>.</value>
    public bool Verbose { get; set; }

    /// <summary>Gets or sets a value indicating whether this command is quiet.</summary>
    /// <value>
    ///   <c>true</c> if quiet; otherwise, <c>false</c>.</value>
    public bool Quiet { get; set; }

    /// <summary>Gets the help text.</summary>
    /// <value>The help text.</value>
    public string HelpText { get; } = "Record changes to the repository.";

    /// <summary>Gets the name.</summary>
    /// <value>The name.</value>
    public string Name { get; } = "commit";

    /// <summary>
    /// Gets the short name.
    /// </summary>
    /// <value>
    /// The short name.
    /// </value>
    public string? ShortName { get; } = "c";

    /// <summary>Configures the specified arguments builder.</summary>
    /// <param name="argumentsBuilder">The arguments builder.</param>
    public void Configure(IArgumentsBuilder argumentsBuilder)
    {
        argumentsBuilder.Separators = Separators.ForAlias('=');
        argumentsBuilder.AddOptional(
            "m",
            "message",
            () => this.Message,
            message => this.Message = message,
            "Use the given <msg> as the commit message.",
            true);
        CommonOptions.ConfigureQuiet(argumentsBuilder, this.Quiet, quiet => this.Quiet = quiet);
        CommonOptions.ConfigureVerbose(argumentsBuilder, this.Verbose, verbose => this.Verbose = verbose);
    }
}