// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVerbBuilder.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System;
using System.Threading.Tasks;
using Sundew.Base.Primitives.Computation;

/// <summary>Interface for building verbs.</summary>
/// <typeparam name="TSuccess">The type of the success.</typeparam>
/// <typeparam name="TError">The type of the error.</typeparam>
public interface IVerbBuilder<TSuccess, TError>
{
    /// <summary>
    /// Adds the verb.
    /// </summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <returns>The verb.</returns>
    TVerb AddVerb<TVerb>(TVerb verb, Func<TVerb, Result<TSuccess, ParserError<TError>>> verbHandler)
        where TVerb : IVerb;

    /// <summary>Adds the verb.</summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <param name="verbBuilderAction">The verb builder action.</param>
    /// <returns>The verb.</returns>
    TVerb AddVerb<TVerb>(TVerb verb, Func<TVerb, Result<TSuccess, ParserError<TError>>> verbHandler, Action<IVerbBuilder<TSuccess, TError>> verbBuilderAction)
        where TVerb : IVerb;

    /// <summary>Adds the verb.</summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <returns>The verb.</returns>
    TVerb AddVerb<TVerb>(
        TVerb verb,
        Func<TVerb, ValueTask<Result<TSuccess, ParserError<TError>>>> verbHandler)
        where TVerb : IVerb;

    /// <summary>Adds the verb.</summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <param name="verbBuilderAction">The verb builder action.</param>
    /// <returns>The verb.</returns>
    TVerb AddVerb<TVerb>(
        TVerb verb,
        Func<TVerb, ValueTask<Result<TSuccess, ParserError<TError>>>> verbHandler,
        Action<IVerbBuilder<TSuccess, TError>>? verbBuilderAction)
        where TVerb : IVerb;
}