// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVerbRegistry.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Verbs;

using System;
using System.Collections.Generic;

internal interface IVerbRegistry<TSuccess, TError> : IArgumentsBuilderProvider
{
    bool HasVerbs { get; }

    IEnumerable<VerbRegistry<TSuccess, TError>> VerbRegistries { get; }

    bool TryGetValue(ReadOnlyMemory<char> verb, out VerbRegistry<TSuccess, TError> verbRegistry);
}