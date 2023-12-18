// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INamedArgumentInfo.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

using System.Text;

internal interface INamedArgumentInfo : IArgumentInfo
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    string? Name { get; }

    /// <summary>
    /// Gets the alias.
    /// </summary>
    /// <value>
    /// The alias.
    /// </value>
    string Alias { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is nesting.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is nesting; otherwise, <c>false</c>.
    /// </value>
    bool IsNesting { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is choice.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is choice; otherwise, <c>false</c>.
    /// </value>
    bool IsChoice { get; }

    /// <summary>
    /// Gets the separators.
    /// </summary>
    Separators Separators { get; }

    /// <summary>
    /// Gets the owner.
    /// </summary>
    /// <value>
    /// The owner.
    /// </value>
    IArgumentMissingInfo? Owner { get; }
}