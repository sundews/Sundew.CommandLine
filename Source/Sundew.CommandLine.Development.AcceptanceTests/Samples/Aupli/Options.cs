// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Development.AcceptanceTests.Samples.Aupli;

/// <summary>
/// Defines the commandLine options and parsing.
/// </summary>
/// <seealso cref="Sundew.CommandLine.IArguments" />
public class Options : IArguments
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Options" /> class.
    /// </summary>
    /// <param name="allowShutdown">if set to <c>true</c> [allow shutdown].</param>
    /// <param name="isLoggingToConsole">if set to <c>true</c> [is logging to console].</param>
    /// <param name="fileLogOptions">The file log options.</param>
    public Options(bool allowShutdown, bool isLoggingToConsole, FileLogOptions? fileLogOptions = null)
    {
        this.AllowShutdown = allowShutdown;
        this.IsLoggingToConsole = isLoggingToConsole;
        this.FileLogOptions = fileLogOptions;
    }

    /// <summary>
    /// Gets a value indicating whether [allow shutdown].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [allow shutdown]; otherwise, <c>false</c>.
    /// </value>
    public bool AllowShutdown { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance is logging to console.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is logging to console; otherwise, <c>false</c>.
    /// </value>
    public bool IsLoggingToConsole { get; private set; }

    /// <summary>
    /// Gets the file log options.
    /// </summary>
    /// <value>
    /// The file log options.
    /// </value>
    public FileLogOptions? FileLogOptions { get; private set; }

    public string HelpText { get; } = "Launches Aupli";

    /// <summary>
    /// Configures the specified arguments builder.
    /// </summary>
    /// <param name="argumentsBuilder">The arguments builder.</param>
    public void Configure(IArgumentsBuilder argumentsBuilder)
    {
        argumentsBuilder.AddOptional(
            "fl",
            "file-log",
            this.FileLogOptions,
            () => new FileLogOptions(default!),
            value => this.FileLogOptions = value,
            "Specifies whether to use a File logger and it's options");

        argumentsBuilder.AddSwitch(
            "cl",
            "console-log",
            this.IsLoggingToConsole,
            argument => this.IsLoggingToConsole = argument,
            "Specifies whether to use a Console logger");

        argumentsBuilder.AddSwitch(
            "s",
            "shutdown",
            this.AllowShutdown,
            argument => this.AllowShutdown = argument,
            "Allows Aupli to shutdown the device when closing.");
    }
}