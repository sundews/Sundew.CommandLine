// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandLineParser.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sundew.Base.Primitives.Computation;

/// <summary>
/// Interface for implementing the command line parser.
/// </summary>
/// <typeparam name="TSuccess">The type of the success.</typeparam>
/// <typeparam name="TError">The type of the error.</typeparam>
public interface ICommandLineParser<TSuccess, TError>
{
    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="startIndex">The start index.</param>
    /// <returns>The parser result.</returns>
    R<TSuccess, ParserError<TError>> Parse(IReadOnlyList<string> arguments, int startIndex);

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    R<TSuccess, ParserError<TError>> Parse(IReadOnlyList<string> arguments);

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="startIndex">The start index.</param>
    /// <returns>The parser result.</returns>
    R<TSuccess, ParserError<TError>> Parse(IReadOnlyList<ReadOnlyMemory<char>> arguments, int startIndex);

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    R<TSuccess, ParserError<TError>> Parse(IReadOnlyList<ReadOnlyMemory<char>> arguments);

    /// <summary>Parses the specified arguments.</summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    R<TSuccess, ParserError<TError>> Parse(string arguments);

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="startIndex">The argument index at which to start parsing.</param>
    /// <returns>The parser result.</returns>
    R<TSuccess, ParserError<TError>> Parse(string arguments, int startIndex);

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    ValueTask<R<TSuccess, ParserError<TError>>> ParseAsync(IReadOnlyList<string> arguments);

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    ValueTask<R<TSuccess, ParserError<TError>>> ParseAsync(IReadOnlyList<ReadOnlyMemory<char>> arguments);

    /// <summary>Parses the specified arguments.</summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    ValueTask<R<TSuccess, ParserError<TError>>> ParseAsync(string arguments);

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="startIndex">The argument index at which to start parsing.</param>
    /// <returns>The parser result.</returns>
    ValueTask<R<TSuccess, ParserError<TError>>> ParseAsync(string arguments, int startIndex);

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="startIndex">The start index.</param>
    /// <returns>The parser result.</returns>
    ValueTask<R<TSuccess, ParserError<TError>>> ParseAsync(IReadOnlyList<ReadOnlyMemory<char>> arguments, int startIndex);
}