// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVerbRegistry.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Verbs
{
    using System.Collections.Generic;

    internal interface IVerbRegistry<TSuccess, TError>
    {
        bool HasVerbs { get; }

        IEnumerable<VerbRegistry<TSuccess, TError>> VerbRegistries { get; }

        bool TryGetValue(string verb, out VerbRegistry<TSuccess, TError> verbRegistry);
    }
}