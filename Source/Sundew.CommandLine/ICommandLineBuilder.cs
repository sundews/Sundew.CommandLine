// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandLineBuilder.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System;
using System.Threading.Tasks;
using Sundew.Base.Primitives.Computation;

/// <summary>
/// Interface for implementing a command line builder.
/// </summary>
/// <typeparam name="TSuccess">The type of the success.</typeparam>
/// <typeparam name="TError">The type of the error.</typeparam>
public interface ICommandLineBuilder<TSuccess, TError>
{
    /// <summary>
    /// Adds the verb.
    /// </summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <returns>The verb.</returns>
    TVerb AddVerb<TVerb>(TVerb verb, Func<TVerb, R<TSuccess, ParserError<TError>>> verbHandler)
        where TVerb : IVerb;

    /// <summary>Adds the verb.</summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <param name="verbBuilderAction">The verb builder action.</param>
    /// <returns>The verb.</returns>
    TVerb AddVerb<TVerb>(TVerb verb, Func<TVerb, R<TSuccess, ParserError<TError>>> verbHandler, Action<IVerbBuilder<TSuccess, TError>> verbBuilderAction)
        where TVerb : IVerb;

    /// <summary>
    /// Specifies the default arguments for parsing (non verb mode).
    /// </summary>
    /// <typeparam name="TArguments">The type of the arguments.</typeparam>
    /// <param name="arguments">The arguments.</param>
    /// <param name="argumentsHandler">The arguments handler.</param>
    /// <returns>The arguments.</returns>
    TArguments WithArguments<TArguments>(TArguments arguments, Func<TArguments, R<TSuccess, ParserError<TError>>> argumentsHandler)
        where TArguments : IArguments;

    /// <summary>
    /// Adds the verb.
    /// </summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <returns>The verb.</returns>
    TVerb AddVerb<TVerb>(TVerb verb, Func<TVerb, ValueTask<R<TSuccess, ParserError<TError>>>> verbHandler)
        where TVerb : IVerb;

    /// <summary>Adds the verb.</summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <param name="verbBuilderAction">The verb builder function.</param>
    /// <returns>The verb.</returns>
    TVerb AddVerb<TVerb>(TVerb verb, Func<TVerb, ValueTask<R<TSuccess, ParserError<TError>>>> verbHandler, Action<IVerbBuilder<TSuccess, TError>>? verbBuilderAction)
        where TVerb : IVerb;

    /// <summary>
    /// Specifies the default arguments for parsing (non verb mode).
    /// </summary>
    /// <typeparam name="TArguments">The type of the arguments.</typeparam>
    /// <param name="arguments">The arguments.</param>
    /// <param name="argumentsHandler">The arguments handler.</param>
    /// <returns>The arguments.</returns>
    TArguments WithArguments<TArguments>(TArguments arguments, Func<TArguments, ValueTask<R<TSuccess, ParserError<TError>>>> argumentsHandler)
        where TArguments : IArguments;
}