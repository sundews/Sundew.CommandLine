// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandLineHelper.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    /// <summary>
    /// Interface for implementing the command line parser.
    /// </summary>
    public interface ICommandLineHelper
    {
        /// <summary>
        /// Creates the help text.
        /// </summary>
        /// <returns>The help text.</returns>
        string CreateHelpText();
    }
}