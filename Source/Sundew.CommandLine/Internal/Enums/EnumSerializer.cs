// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumSerializer.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sundew.Base;
using Sundew.Base.Memory;
using Sundew.Base.Text;

internal class EnumSerializer<TEnum>
    where TEnum : Enum
{
    private const string EnumValueSeparator = ", ";
    private readonly Dictionary<ReadOnlyMemory<char>, TEnum> values = new(ReadOnlyMemoryCharEqualityComparer.Instance);
    private readonly Dictionary<TEnum, ReadOnlyMemory<char>> enums = new();
    private readonly Dictionary<TEnum, ReadOnlyMemory<char>> shortEnums = new();
    private readonly string[] possibleValues;

    public EnumSerializer(IEnumerable<TEnum> enumOptions)
    {
        var actualEnumOptions = enumOptions.ToList();
        foreach (var enumOption in actualEnumOptions)
        {
            var name = Enum.GetName(typeof(TEnum), enumOption);
            var fullName = name.AsMemory().Split(
                (character, index, splitContext) =>
                {
                    if (index == 0)
                    {
                        splitContext.Append(char.ToLowerInvariant(character));
                        return SplitAction.Ignore;
                    }

                    if (char.IsUpper(character))
                    {
                        splitContext.Append('-');
                        splitContext.Append(char.ToLowerInvariant(character));
                        return SplitAction.Ignore;
                    }

                    return SplitAction.Include;
                },
                SplitOptions.RemoveEmptyEntries).Single();
            this.values.Add(fullName, enumOption);
            this.enums.Add(enumOption, fullName);
            var shortName = fullName.Slice(0, 1);
            if (this.values.ContainsKey(shortName))
            {
                if (fullName.Length > 6)
                {
                    var dashIndex = fullName.Span.IndexOf('-');
                    if (dashIndex > -1)
                    {
                        shortName = fullName.Slice(0, dashIndex);
                    }
                }
            }

            if (!this.values.ContainsKey(shortName))
            {
                this.values.Add(shortName, enumOption);
                this.shortEnums.Add(enumOption, shortName);
            }
        }

        var stringBuilder = new StringBuilder();
        this.possibleValues = new string[actualEnumOptions.Count + 1];
        for (var index = 0; index < actualEnumOptions.Count; index++)
        {
            var enumOption = actualEnumOptions[index];
            var name = this.enums[enumOption].ToString();
            if (this.shortEnums.TryGetValue(enumOption, out var shortName))
            {
                name = $"[{shortName}]{name.Substring(shortName.Length, name.Length - shortName.Length)}";
            }

            this.possibleValues[index + 1] = name;
            stringBuilder.Append(name);
            stringBuilder.Append(EnumValueSeparator);
        }

        this.possibleValues[0] = stringBuilder.ToString(EnumValueSeparator);
    }

    public ReadOnlySpan<char> Serialize(TEnum enumOption)
    {
        return this.enums[enumOption].Span;
    }

    public TEnum Deserialize(ReadOnlySpan<char> value)
    {
        var valueAsString = value.ToString();
        if (this.values.TryGetValue(valueAsString.AsMemory(), out var enumValue) || valueAsString.TryParseEnum<TEnum>(out enumValue, true))
        {
            return enumValue;
        }

        throw new ArgumentException($"{valueAsString} didn't match a value in enum type {typeof(TEnum)}");
    }

    public string[] GetAllValues()
    {
        return this.possibleValues;
    }
}