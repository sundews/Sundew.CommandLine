// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOption.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

using System;
using System.Text;
using Sundew.Base;

internal interface IOption : INamedArgumentInfo, IArgumentHelpInfo, IArgumentMissingInfo
{
    R<bool, GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings, bool useAliases);

    RwE<ParserError> DeserializeFrom(CommandLineArgumentsParser commandLineArgumentsParser, ArgumentList argumentList, ReadOnlySpan<char> value, Settings settings);

    void AppendDefaultText(StringBuilder stringBuilder, Settings settings, bool isNested);
}