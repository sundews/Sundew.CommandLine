// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Install.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Git.CommandLine.LargeFileSystem
{
    using Sundew.CommandLine;

    /// <summary>Ensures that git lfs is setup properly.</summary>
    /// <seealso cref="Sundew.Git.CommandLine.LargeFileSystem.ILfsVerb" />
    public class Install : ILfsVerb
    {
        /// <summary>Gets the next verb.</summary>
        /// <value>The next verb.</value>
        public IVerb? NextVerb => null;

        /// <summary>Gets the help text.</summary>
        /// <value>The help text.</value>
        public string HelpText { get; } = "Installs lfs into git hooks";

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; } = "install";

        /// <summary>Configures the specified arguments builder.</summary>
        /// <param name="argumentsBuilder">The arguments builder.</param>
        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
        }
    }
}