// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValue.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

using System;
using System.Text;
using Sundew.Base;

internal interface IValue : IArgumentInfo, IArgumentMissingInfo
{
    bool IsRequired { get; }

    bool IsList { get; }

    string Name { get; }

    string? DefaultValueHelpText { get; }

    RwE<ParserError> DeserializeFrom(ReadOnlySpan<char> argument, ArgumentList argumentList, Settings settings);

    RwE<GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings);
}