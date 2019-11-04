// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentSeparation.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    /// <summary>Specifies how arguments and values are separated from each other.</summary>
    public enum ArgumentSeparation
    {
        /// <summary>  The separation is used for the name.</summary>
        Name = 1,

        /// <summary>  The separation is used for the alias.</summary>
        Alias = 2,

        /// <summary>  The separation is used for the name and the alias.</summary>
        NameAndAlias = Name | Alias,
    }
}