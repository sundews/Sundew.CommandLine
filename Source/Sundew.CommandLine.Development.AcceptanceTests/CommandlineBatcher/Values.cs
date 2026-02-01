// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Values.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Development.AcceptanceTests.CommandlineBatcher;

using System;

public class Values
{
    public Values(params string[] arguments)
    {
        this.Arguments = arguments;
    }

    public string[] Arguments { get; }

    public static Values From(string value, string batchValueSeparator)
    {
        return new(value.Split(batchValueSeparator, StringSplitOptions.RemoveEmptyEntries));
    }
}