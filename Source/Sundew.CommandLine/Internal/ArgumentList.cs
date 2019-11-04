// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentList.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal class ArgumentList : IEnumerable<string>
    {
        private readonly IReadOnlyList<string> arguments;
        private int index;

        public ArgumentList(IReadOnlyList<string> arguments, int index)
        {
            this.arguments = arguments;
            this.index = index;
        }

        public bool TryPeek([MaybeNullWhen(false), NotNullWhen(true)]out string argument)
        {
            if (this.index + 1 < this.arguments.Count)
            {
                argument = this.arguments[this.index];
                return true;
            }

            argument = default!;
            return false;
        }

        public bool TryMoveNext([MaybeNullWhen(false), NotNullWhen(true)]out string argument)
        {
            this.index++;
            if (this.index < this.arguments.Count)
            {
                argument = this.arguments[this.index];
                return true;
            }

            argument = default!;
            return false;
        }

        public void MoveBack()
        {
            this.index--;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.GetEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private IEnumerable<string> GetEnumerable()
        {
            for (; this.index < this.arguments.Count; this.index++)
            {
                yield return this.arguments[this.index];
            }
        }
    }
}