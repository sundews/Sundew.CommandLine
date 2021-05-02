// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Lfs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Git.CommandLine.LargeFileSystem
{
    using Sundew.CommandLine;

    /// <summary>Sub commands for git large file system.</summary>
    /// <seealso cref="Sundew.CommandLine.IVerb" />
    public class Lfs : IVerb
    {
        /// <summary>Initializes a new instance of the <see cref="Lfs"/> class.</summary>
        /// <param name="lfsVerb">The LFS verb.</param>
        public Lfs(ILfsVerb lfsVerb)
        {
            this.NextVerb = lfsVerb;
            this.Install = default!;
            this.Track = default!;
            this.Untrack = default!;
        }

        /// <summary>Initializes a new instance of the <see cref="Lfs"/> class.</summary>
        public Lfs()
        {
            this.Install = new Install();
            this.Track = new Track();
            this.Untrack = new Untrack();
        }

        /// <summary>Gets the next verb.</summary>
        /// <value>The next verb.</value>
        public IVerb? NextVerb { get; }

        /// <summary>Gets the install.</summary>
        /// <value>The install.</value>
        public Install Install { get; }

        /// <summary>Gets the track.</summary>
        /// <value>The track.</value>
        public Track Track { get; }

        /// <summary>Gets the untrack.</summary>
        /// <value>The untrack.</value>
        public Untrack Untrack { get; }

        /// <summary>Gets the help text.</summary>
        /// <value>The help text.</value>
        public string HelpText { get; } = @"Git Large File Storage (LFS) replaces large files
such as audio samples, videos, datasets, and graphics
with text pointers inside Git, while storing the file
contents on a remote server like GitHub.com or GitHub Enterprise.";

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; } = "lfs";

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
        }
    }
}