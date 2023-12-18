// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Push.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Git.CommandLine;

using Sundew.CommandLine;

/// <summary>Update remote refs along with associated objects.</summary>
/// <seealso cref="Sundew.CommandLine.IVerb" />
public class Push : IVerb
{
    /// <summary>Initializes a new instance of the <see cref="Push"/> class.</summary>
    public Push()
    {
        this.Repository = default!;
    }

    /// <summary>Initializes a new instance of the <see cref="Push"/> class.</summary>
    /// <param name="repository">The repository.</param>
    public Push(Repository repository)
    {
        this.Repository = repository;
    }

    /// <summary>Gets the next verb.</summary>
    /// <value>The next verb.</value>
    public IVerb? NextVerb => null;

    /// <summary>Gets the help text.</summary>
    /// <value>The help text.</value>
    public string HelpText { get; } = "Update remote refs along with associated objects.";

    /// <summary>Gets the name.</summary>
    /// <value>The name.</value>
    public string Name { get; } = "push";

    /// <summary>
    /// Gets the short name.
    /// </summary>
    /// <value>
    /// The short name.
    /// </value>
    public string? ShortName { get; } = null;

    /// <summary>Gets or sets a value indicating whether verbose logging is used.</summary>
    /// <value>A value indicating whether verbose logging is used.</value>
    public bool Verbose { get; set; }

    /// <summary>Gets or sets the push option.</summary>
    /// <value>The push option.</value>
    public string? PushOption { get; set; }

    /// <summary>Gets the repository.</summary>
    /// <value>The repository.</value>
    public Repository Repository { get; private set; }

    /// <summary>Configures the specified arguments builder.</summary>
    /// <param name="argumentsBuilder">The arguments builder.</param>
    public void Configure(IArgumentsBuilder argumentsBuilder)
    {
        argumentsBuilder.Separators = Separators.ForAlias('=');
        argumentsBuilder.AddOptional(
            "o",
            "push-option",
            () => this.PushOption,
            pushOption => this.PushOption = pushOption,
            "Transmit the given string to the server, which passes them to the pre-receive as well as the post-receive hook.",
            false);
        CommonOptions.ConfigureVerbose(argumentsBuilder, this.Verbose, verbose => this.Verbose = verbose);
        argumentsBuilder.AddOptionalValue(
            "repository refspec",
            () => this.Repository?.ToString(),
            repository => this.Repository = Repository.Parse(repository),
            "The repository and refspec.",
            true);
    }
}