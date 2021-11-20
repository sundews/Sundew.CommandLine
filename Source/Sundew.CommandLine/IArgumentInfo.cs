// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IArgumentInfo.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System.Collections.Generic;
using System.Globalization;

/// <summary>
/// Interface for implementing getting argument info.
/// </summary>
public interface IArgumentInfo
{
    /// <summary>Gets the usage.</summary>
    /// <value>The usage.</value>
    string Usage { get; }

    /// <summary>
    /// Gets the help lines.
    /// </summary>
    /// <value>
    /// The help lines.
    /// </value>
    IReadOnlyList<string> HelpLines { get; }

    /// <summary>Resets to default.</summary>
    /// <param name="cultureInfo">The culture information.</param>
    void ResetToDefault(CultureInfo cultureInfo);
}