// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IArgumentHelpInfo.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    using System.Text;

    internal interface IArgumentHelpInfo : IArgumentMissingInfo
    {
        bool IsRequired { get; }

        int Index { get; }

        void AppendHelpText(StringBuilder stringBuilder, Settings settings, int indent, int nameMaxLength, int aliasMaxLength, int helpTextMaxLength, bool isForVerb, bool isForNested);
    }
}