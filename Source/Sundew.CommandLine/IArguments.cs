// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IArguments.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    /// <summary>
    /// Interface for configuring verbs and arguments.
    /// </summary>
    public interface IArguments
    {
        /// <summary>
        /// Configures the specified arguments builder.
        /// </summary>
        /// <param name="argumentsBuilder">The arguments builder.</param>
        void Configure(IArgumentsBuilder argumentsBuilder);
    }
}