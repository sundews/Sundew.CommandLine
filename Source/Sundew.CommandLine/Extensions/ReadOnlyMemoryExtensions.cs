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
    using Sundew.Base.Text;

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
        public static IEnumerable<string> ParseCommandLineArguments(this ReadOnlyMemory<char> input)
        {
            const char doubleQuote = '\"';
            const char slash = '\\';
            const char space = ' ';
            var isInQuote = false;
            var isInEscape = false;
            return input.Split(
                (character, index, _) =>
                {
                    var actualIsInEscape = isInEscape;
                    isInEscape = false;
                    switch (character)
                    {
                        case slash:
                            var nextIndex = index + 1;
                            if (isInQuote && input.Length > nextIndex && input.Span[nextIndex] == doubleQuote)
                            {
                                isInEscape = true;
                                return SplitAction.Ignore;
                            }

                            return SplitAction.Include;
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