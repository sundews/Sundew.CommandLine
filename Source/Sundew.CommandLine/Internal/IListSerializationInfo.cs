// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListSerializationInfo.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

using System.Collections.Generic;

internal interface IListSerializationInfo<TItem>
{
    IList<TItem> List { get; }

    Serialize<TItem> Serialize { get; }

    bool UseDoubleQuotes { get; }
}