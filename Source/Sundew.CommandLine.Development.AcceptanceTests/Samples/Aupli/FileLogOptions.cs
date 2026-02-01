// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileLogOptions.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Development.AcceptanceTests.Samples.Aupli;

/// <summary>
/// Options for the file logger.
/// </summary>
/// <seealso cref="Sundew.CommandLine.IArguments" />
public class FileLogOptions : IArguments
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileLogOptions" /> class.
    /// </summary>
    /// <param name="logPath">The log path.</param>
    /// <param name="maxLogFileSizeInBytes">The maximum log file size in bytes.</param>
    /// <param name="maxNumberOfLogFiles">The maximum number of log files.</param>
    public FileLogOptions(string logPath, long maxLogFileSizeInBytes = 5_000_000, int maxNumberOfLogFiles = 10)
    {
        this.LogPath = logPath;
        this.MaxLogFileSizeInBytes = maxLogFileSizeInBytes;
        this.MaxNumberOfLogFiles = maxNumberOfLogFiles;
    }

    /// <summary>
    /// Gets the log path.
    /// </summary>
    /// <value>
    /// The log path.
    /// </value>
    public string LogPath { get; private set; }

    /// <summary>
    /// Gets the maximum log file size in bytes.
    /// </summary>
    /// <value>
    /// The maximum log file size in bytes.
    /// </value>
    public long MaxLogFileSizeInBytes { get; private set; }

    /// <summary>
    /// Gets the maximum number of log files.
    /// </summary>
    /// <value>
    /// The maximum number of log files.
    /// </value>
    public int MaxNumberOfLogFiles { get; private set; }

    public string HelpText { get; } = "File logging options.";

    /// <summary>
    /// Configures the specified arguments builder.
    /// </summary>
    /// <param name="argumentsBuilder">The arguments builder.</param>
    public void Configure(IArgumentsBuilder argumentsBuilder)
    {
        argumentsBuilder.AddRequired(
            "lp",
            "log-path",
            () => this.LogPath,
            argument => this.LogPath = argument,
            "Specifies the log path, in case of the File logger",
            true);

        argumentsBuilder.AddOptional(
            "ms",
            "max-size",
            () => this.MaxLogFileSizeInBytes.ToString(),
            argument => this.MaxLogFileSizeInBytes = long.Parse(argument),
            "Specifies max log file size in bytes.");

        argumentsBuilder.AddOptional(
            "mf",
            "max-files",
            () => this.MaxNumberOfLogFiles.ToString(),
            argument => this.MaxNumberOfLogFiles = int.Parse(argument),
            "Specifies max number of log files.");
    }
}