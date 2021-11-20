// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyMemoryCharEqualityComparer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

using System;
using System.Collections.Generic;
using Sundew.Base.Equality;

internal class ReadOnlyMemoryCharEqualityComparer : IEqualityComparer<ReadOnlyMemory<char>>
{
    public static ReadOnlyMemoryCharEqualityComparer Instance { get; } = new ReadOnlyMemoryCharEqualityComparer();

    public bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
    {
        return x.Span.SequenceEqual(y.Span);
    }

    public int GetHashCode(ReadOnlyMemory<char> obj)
    {
        return EqualityHelper.GetItemsHashCode(obj.Span);
    }
}