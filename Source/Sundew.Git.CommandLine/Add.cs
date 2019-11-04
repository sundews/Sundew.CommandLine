// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Add.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Git.CommandLine
{
    using Sundew.CommandLine;

    /// <summary>Add file contents to the index.</summary>
    /// <seealso cref="Sundew.CommandLine.IVerb" />
    public class Add : IVerb
    {
        /// <summary>Initializes a new instance of the <see cref="Add"/> class.</summary>
        public Add()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Add"/> class.</summary>
        /// <param name="directoryOrFilePath">The directory or file path.</param>
        public Add(string directoryOrFilePath)
        {
            this.DirectoryOrFilePath = directoryOrFilePath;
        }

        /// <summary>Gets the next verb.</summary>
        /// <value>The next verb.</value>
        public IVerb NextVerb => null;

        /// <summary>Gets the directory or file path.</summary>
        /// <value>The directory or file path.</value>
        public string DirectoryOrFilePath { get; private set; }

        /// <summary>Gets or sets a value indicating whether this command is verbose.</summary>
        /// <value>
        ///   <c>true</c> if verbose; otherwise, <c>false</c>.</value>
        public bool Verbose { get; set; }

        /// <summary>Gets the help text.</summary>
        /// <value>The help text.</value>
        public string HelpText { get; } = "Add/stage files for commit.";

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; } = "add";

        /// <summary>Configures the specified arguments builder.</summary>
        /// <param name="argumentsBuilder">The arguments builder.</param>
        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            CommonOptions.ConfigureVerbose(argumentsBuilder, this.Verbose, verbose => this.Verbose = verbose);
            argumentsBuilder.AddOptionalValue(
                "directory or file path",
                () => this.DirectoryOrFilePath,
                value => this.DirectoryOrFilePath = value,
                "Files to add content from.",
                true);
        }
    }
}