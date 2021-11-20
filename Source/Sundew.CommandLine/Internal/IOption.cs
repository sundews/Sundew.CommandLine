// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOption.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

using System;
using System.Text;
using Sundew.Base.Primitives.Computation;

internal interface IOption : INamedArgumentInfo, IArgumentHelpInfo, IArgumentMissingInfo
{
    Result<bool, GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings, bool useAliases);

    Result.IfError<ParserError> DeserializeFrom(CommandLineArgumentsParser commandLineArgumentsParser, ArgumentList argumentList, ReadOnlySpan<char> value, Settings settings);

    void AppendDefaultText(StringBuilder stringBuilder, Settings settings, bool isNested);
}