// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Extensions;

using System;
using System.Diagnostics.CodeAnalysis;

internal static class StringExtensions
{
    public static int GetAdjustedLengthOrDefault(this string? input, int lengthAdjustment, int defaultValue)
    {
        if (string.IsNullOrEmpty(input))
        {
            return defaultValue;
        }

        return input!.Length + lengthAdjustment;
    }

    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? input)
    {
        return string.IsNullOrEmpty(input);
    }

    public static string TransformIfNotNullOrEmpty(this string? input, Func<string, string> transformFunc, Func<string>? defaultValueFunc)
    {
        return input.IsNullOrEmpty() ? defaultValueFunc?.Invoke() ?? string.Empty : transformFunc(input);
    }

    public static string TransformIfNotNullOrEmpty(this string? input, Func<string, string> transformFunc, string? defaultValue = null)
    {
        return input.IsNullOrEmpty() ? defaultValue ?? string.Empty : transformFunc(input);
    }
}