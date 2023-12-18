// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IArgumentHelpInfo.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System.Text;
using Sundew.CommandLine.Internal;

internal interface IArgumentHelpInfo
{
    bool IsRequired { get; }

    int Index { get; }

    void AppendHelpText(StringBuilder stringBuilder, Settings settings, int indent, TextSizes textSizes, bool isForVerb, bool isForNested);
}