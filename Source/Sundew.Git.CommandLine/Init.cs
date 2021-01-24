// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Init.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Git.CommandLine
{
    using Sundew.CommandLine;

    /// <summary>Create an empty Git repository or reinitialize an existing one.</summary>
    /// <seealso cref="CommonOptions" />
    /// <seealso cref="Sundew.CommandLine.IVerb" />
    public class Init : IVerb
    {
        /// <summary>Initializes a new instance of the <see cref="Init"/> class.</summary>
        public Init()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Init"/> class.</summary>
        /// <param name="directoryPath">The directory path.</param>
        public Init(string? directoryPath)
        {
            this.DirectoryPath = directoryPath;
        }

        /// <summary>Gets the next verb.</summary>
        /// <value>The next verb.</value>
        public IVerb? NextVerb => null;

        /// <summary>Gets the directory path.</summary>
        /// <value>The directory path.</value>
        public string? DirectoryPath { get; private set; }

        /// <summary>Gets or sets the template directory.</summary>
        /// <value>The template directory.</value>
        public string? TemplateDirectory { get; set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="Init"/> is bare.</summary>
        /// <value>
        ///   <c>true</c> if bare; otherwise, <c>false</c>.</value>
        public bool Bare { get; set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="CommonOptions"/> is quiet.</summary>
        /// <value>
        ///   <c>true</c> if quiet; otherwise, <c>false</c>.</value>
        public bool Quiet { get; set; }

        /// <summary>Gets the help text.</summary>
        /// <value>The help text.</value>
        public string HelpText { get; } = "Create an empty Git repository or reinitialize an existing one.";

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; } = "init";

        /// <summary>Configures the specified arguments builder.</summary>
        /// <param name="argumentsBuilder">The arguments builder.</param>
        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.Separators = Separators.ForAlias('=');
            CommonOptions.ConfigureQuiet(argumentsBuilder, this.Quiet, quiet => this.Quiet = quiet);
            argumentsBuilder.AddSwitch(
                null,
                "bare",
                this.Bare,
                value => this.Bare = value,
                "Create a bare repository. If GIT_DIR environment is not set, it is set to the current working directory.");

            argumentsBuilder.AddOptional(
                null,
                "template",
                () => this.TemplateDirectory,
                (value) => this.TemplateDirectory = value,
                @"Specify the directory from which templates will be used. (See the ""TEMPLATE DIRECTORY"" section below.)",
                true);

            argumentsBuilder.AddOptionalValue(
                "directory path",
                () => this.DirectoryPath,
                (value) => this.DirectoryPath = value,
                "Create empty Git repository in the specified directory.",
                true);
        }
    }
}