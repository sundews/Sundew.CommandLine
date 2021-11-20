// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextSizes.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

internal ref struct TextSizes
{
    public TextSizes(int verbNameMaxLength, int nameMaxLength, int aliasMaxLength, int helpTextMaxLength, int valuesMaxLength)
    {
        this.VerbNameMaxLength = verbNameMaxLength;
        this.NameMaxLength = nameMaxLength;
        this.AliasMaxLength = aliasMaxLength;
        this.HelpTextMaxLength = helpTextMaxLength;
        this.ValuesMaxLength = valuesMaxLength;
    }

    public int VerbNameMaxLength { get; set; }

    public int NameMaxLength { get; set; }

    public int AliasMaxLength { get; set; }

    public int HelpTextMaxLength { get; set; }

    public int ValuesMaxLength { get; set; }
}