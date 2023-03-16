// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandLineGenerator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using Sundew.Base.Primitives.Computation;

/// <summary>
/// Interface for implementing a command line generator.
/// </summary>
public interface ICommandLineGenerator
{
    /// <summary>Generates the specified verb.</summary>
    /// <param name="verb">The verb.</param>
    /// <returns>The generator result.</returns>
    R<string, GeneratorError> Generate(IVerb verb);

    /// <summary>Generates the specified use aliases.</summary>
    /// <param name="verb">The verb.</param>
    /// <param name="useAliases">if set to <c>true</c> [use aliases].</param>
    /// <returns>The generator result.</returns>
    R<string, GeneratorError> Generate(IVerb verb, bool useAliases);

    /// <summary>Generates the specified verb.</summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The generator result.</returns>
    R<string, GeneratorError> Generate(IArguments arguments);

    /// <summary>
    /// Generates the specified verb.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="useAliases">if set to <c>true</c> [use aliases].</param>
    /// <returns>
    /// The generator result.
    /// </returns>
    R<string, GeneratorError> Generate(IArguments arguments, bool useAliases);
}