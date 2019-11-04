// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Extensions
{
    using System;
    using System.Collections.Generic;
    using Sundew.Base.Text;

    internal static class StringExtensions
    {
        public static IEnumerable<string> SplitBasedCommandLineTokenizer(this ReadOnlyMemory<char> input)
        {
            const char doubleQuote = '\"';
            const char space = ' ';
            var isInQuote = false;
            var isInEscape = false;
            return input.Split(
                (character, index, stringBuilder) =>
                {
                    var actualIsInEscape = isInEscape;
                    isInEscape = false;
                    switch (character)
                    {
                        case doubleQuote:
                            if (!actualIsInEscape)
                            {
                                isInEscape = true;
                                isInQuote = true;
                            }

                            return actualIsInEscape ? SplitAction.Include : SplitAction.Ignore;
                        case space:
                            if (actualIsInEscape)
                            {
                                isInQuote = false;
                                return SplitAction.Split;
                            }

                            return isInQuote ? SplitAction.Include : SplitAction.Split;
                        default:
                            return SplitAction.Include;
                    }
                },
                StringSplitOptions.RemoveEmptyEntries);
        }
    }
}