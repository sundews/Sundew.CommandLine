﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IArguments.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

/// <summary>
/// Interface for configuring verbs and arguments.
/// </summary>
public interface IArguments
{
    /// <summary>Gets the help text.</summary>
    /// <value>The help text.</value>
    string HelpText { get; }

    /// <summary>
    /// Configures the specified arguments builder.
    /// </summary>
    /// <param name="argumentsBuilder">The arguments builder.</param>
    void Configure(IArgumentsBuilder argumentsBuilder);
}