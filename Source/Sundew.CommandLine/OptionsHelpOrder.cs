// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionsHelpOrder.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    /// <summary>
    /// Determines how the arguments are ordered in the help.
    /// </summary>
    public enum OptionsHelpOrder
    {
        /// <summary>
        /// Required parameters are shown first, after which the follow the order in which they were added.
        /// </summary>
        RequiredFirst,

        /// <summary>
        /// The help is constructed by the order in which it was added.
        /// </summary>
        AsAdded,
    }
}