// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System.Globalization;

/// <summary>
/// Defines the settings for the command line parser, generator and help generator.
/// </summary>
public class Settings
{
    /// <summary>Initializes a new instance of the <see cref="Settings"/> class.</summary>
    /// <param name="separators">The separators.</param>
    /// <param name="cultureInfo">The culture information.</param>
    public Settings(Separators separators = default, CultureInfo? cultureInfo = default)
    {
        this.Separators = separators.Equals(default) ? Separators.Create() : separators;
        this.CultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;
    }

    /// <summary>
    /// Gets the default separators.
    /// </summary>
    public Separators Separators { get; }

    /// <summary>Gets the culture information.</summary>
    /// <value>The culture information.</value>
    public CultureInfo CultureInfo { get; }

    /// <summary>
    /// sasd.
    /// </summary>
    public class Wooooaaw
    {
    }
}