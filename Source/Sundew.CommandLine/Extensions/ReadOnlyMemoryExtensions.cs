// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyMemoryExtensions.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Extensions
{
    using System;
    using System.Collections.Generic;
    using Sundew.Base.Memory;

    /// <summary>
    /// Extends the ReadOnlyMemory of char with Command Line parsing.
    /// </summary>
    public static class ReadOnlyMemoryExtensions
    {
        /// <summary>
        /// Parses the command line arguments.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The arguments.</returns>
        public static IEnumerable<ReadOnlyMemory<char>> ParseCommandLineArguments(this ReadOnlyMemory<char> input)
        {
            const char doubleQuote = '\"';
            const char slash = '\\';
            const char space = ' ';
            var isInQuote = false;
            var isInEscape = false;
            var previousWasSpace = false;
            return input.Split(
                (character, index, splitContext) =>
                {
                    var actualIsInEscape = isInEscape;
                    var actualPreviousWasSpace = previousWasSpace;
                    isInEscape = false;
                    previousWasSpace = false;
                    switch (character)
                    {
                        case slash:
                            if (splitContext.GetNextOrDefault(index) == doubleQuote)
                            {
                                isInEscape = true;
                                return SplitAction.Ignore;
                            }

                            return SplitAction.Include;
                        case doubleQuote:
                            if (!actualIsInEscape)
                            {
                                isInQuote = !isInQuote;
                            }

                            return actualIsInEscape ? SplitAction.Include : SplitAction.Ignore;
                        case space:
                            previousWasSpace = true;
                            if (actualIsInEscape)
                            {
                                isInQuote = false;
                                return SplitAction.Split;
                            }

                            if (actualPreviousWasSpace)
                            {
                                return SplitAction.Ignore;
                            }

                            return isInQuote ? SplitAction.Include : SplitAction.Split;
                        default:
                            return SplitAction.Include;
                    }
                },
                SplitOptions.None);
        }
    }
}