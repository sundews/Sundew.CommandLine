// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IArgumentsBuilderProvider.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

internal interface IArgumentsBuilderProvider
{
    ArgumentsBuilder Builder { get; }
}