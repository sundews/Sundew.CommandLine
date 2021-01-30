// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValue.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System;
    using System.Text;
    using Sundew.Base.Computation;

    internal interface IValue : IArgumentInfo
    {
        bool IsRequired { get; }

        bool IsList { get; }

        string Name { get; }

        string? DefaultValueHelpText { get; }

        Result.IfError<ParserError> DeserializeFrom(ReadOnlySpan<char> argument, ArgumentList argumentList, Settings settings);

        Result.IfError<GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings);
    }
}