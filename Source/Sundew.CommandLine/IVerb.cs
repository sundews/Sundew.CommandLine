// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    /// <summary>
    /// Interface for implementing verbs for the command line.
    /// </summary>
    /// <seealso cref="Sundew.CommandLine.IArguments" />
    public interface IVerb : IArguments
    {
        /// <summary>Gets the next verb.</summary>
        /// <value>The next verb.</value>
        IVerb? NextVerb { get; }

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the short name.
        /// </summary>
        /// <value>
        /// The short name.
        /// </value>
        string? ShortName { get; }
    }
}