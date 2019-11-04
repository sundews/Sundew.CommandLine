// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringBuilderExtensions.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Extensions
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class StringBuilderExtensions
    {
        public static StringBuilder Append(this StringBuilder stringBuilder, ReadOnlySpan<char> span)
        {
            if (span.Length > 0)
            {
                unsafe
                {
                    fixed (char* charPointer = &MemoryMarshal.GetReference(span))
                    {
                        stringBuilder.Append(charPointer, span.Length);
                    }
                }
            }

            return stringBuilder;
        }

        public static StringBuilder AppendLine(this StringBuilder stringBuilder, ReadOnlySpan<char> span)
        {
            stringBuilder.Append(span);
            stringBuilder.AppendLine();
            return stringBuilder;
        }
    }
}