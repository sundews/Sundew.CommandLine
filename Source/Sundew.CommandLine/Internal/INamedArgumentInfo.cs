// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INamedArgumentInfo.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    internal interface INamedArgumentInfo : IArgumentInfo
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        string Alias { get; }

        /// <summary>
        /// Gets the separators.
        /// </summary>
        Separators Separators { get; }
    }
}