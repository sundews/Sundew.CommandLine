// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentRegistry.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class ArgumentRegistry<TValue> : IEnumerable<TValue>
    {
        private readonly IReadOnlyDictionary<ReadOnlyMemory<char>, TValue> dictionary;

        public ArgumentRegistry(IReadOnlyDictionary<ReadOnlyMemory<char>, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }

        public bool TryGet(ReadOnlyMemory<char> key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return this.dictionary.Values.Distinct().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}