﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultExtensions.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System;
using Sundew.Base;

/// <summary>Contains extension methods for command line parser results.</summary>
public static class ResultExtensions
{
    /// <summary>Prints the specified result.</summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="result">The result.</param>
    /// <returns>A value indicating whether help could be useful.</returns>
    public static bool WriteToConsole<TValue, TError>(this R<TValue, ParserError<TError>> result)
    {
        if (result.IsSuccess)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(result.Value);
            Console.ResetColor();
        }
        else
        {
            if (result.Error.Type != ParserErrorType.HelpRequested)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine(result.Error);
            Console.ResetColor();
        }

        return !result;
    }

    /// <summary>Gets the exit code.</summary>
    /// <param name="result">The result.</param>
    /// <returns>The exit code.</returns>
    public static int GetExitCode(this R<int, ParserError<int>> result)
    {
        if (result.IsSuccess)
        {
            return result.Value;
        }

        return result.Error.Info;
    }
}