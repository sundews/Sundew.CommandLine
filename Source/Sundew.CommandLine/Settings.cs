// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    using System.Globalization;

    /// <summary>
    /// Defines the settings for the command line parser, generator and help generator.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Gets or sets the default separators.
        /// </summary>
        public Separators Separators { get; set; }

        /// <summary>Gets or sets the culture information.</summary>
        /// <value>The culture information.</value>
        public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;
    }
}