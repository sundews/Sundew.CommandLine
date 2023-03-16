// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOption.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Options;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Sundew.Base.Collections;
using Sundew.Base.Primitives.Computation;
using Sundew.CommandLine.Internal.Helpers;

internal class ListOption<TValue> : IOption, IListSerializationInfo<TValue>
{
    private readonly Deserialize<TValue> deserialize;
    private readonly string? defaultValueHelpText;

    public ListOption(
        string? name,
        string alias,
        IList<TValue> list,
        Serialize<TValue> serialize,
        Deserialize<TValue> deserialize,
        bool isRequired,
        string helpText,
        bool useDoubleQuotes,
        string? defaultValueHelpText,
        int index,
        IArgumentMissingInfo? owner)
    {
        this.Name = name;
        this.Alias = alias;
        this.List = list;
        this.DefaultList = this.List.ToList();
        this.Serialize = serialize;
        this.deserialize = deserialize;
        this.defaultValueHelpText = defaultValueHelpText;
        this.Index = index;
        this.Owner = owner;
        this.UseDoubleQuotes = useDoubleQuotes;
        this.IsRequired = isRequired;
        this.HelpLines = HelpTextHelper.GetHelpLines(helpText);
        this.Usage = HelpTextHelper.GetUsage(name, alias);
    }

    public string? Name { get; }

    public string Alias { get; }

    public Separators Separators => default;

    public IList<TValue> List { get; }

    public IList<TValue> DefaultList { get; }

    public Serialize<TValue> Serialize { get; }

    public bool UseDoubleQuotes { get; }

    public bool IsRequired { get; }

    public int Index { get; }

    public IArgumentMissingInfo? Owner { get; }

    public string Usage { get; }

    public IReadOnlyList<string> HelpLines { get; }

    public bool IsNesting => false;

    public bool IsChoice => this.Owner != null;

    public R<bool, GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings, bool useAliases)
    {
        var wasSerialized = SerializationHelper.SerializeTo(this, this.List, stringBuilder, settings, () =>
        {
            SerializationHelper.AppendNameOrAlias(stringBuilder, this.Name, this.Alias, useAliases);
            stringBuilder.Append(Constants.SpaceCharacter);
        });
        if (!wasSerialized && this.IsRequired)
        {
            return R.Error(new GeneratorError(this, GeneratorErrorType.RequiredOptionMissing));
        }

        return R.Success(wasSerialized);
    }

    public R<ParserError> DeserializeFrom(
        CommandLineArgumentsParser commandLineArgumentsParser,
        ArgumentList argumentList,
        ReadOnlySpan<char> value,
        Settings settings)
    {
        this.List.Clear();
        var currentResult = this.DeserializeFrom(value, settings);
        if (argumentList.TryMoveNext(out _))
        {
            foreach (var argument in argumentList)
            {
                var argumentSpan = argument.Span;
                if (argumentSpan[0] == Constants.ArgumentStartCharacter)
                {
                    argumentList.MoveBack();
                    break;
                }

                currentResult = this.DeserializeFrom(CommandLineArgumentsParser.RemoveValueEscapeIfNeeded(argumentSpan), settings);
            }
        }

        return currentResult;
    }

    public void AppendHelpText(StringBuilder stringBuilder, Settings settings, int indent, TextSizes textSizes, bool isForVerb, bool isForNested)
    {
        HelpTextHelper.AppendHelpText(stringBuilder, settings, this, indent, textSizes.NameMaxLength, textSizes.AliasMaxLength, textSizes.HelpTextMaxLength, isForVerb, isForNested);
    }

    public void AppendMissingArgumentsHint(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine(this.Usage);
    }

    public void ResetToDefault(CultureInfo cultureInfo)
    {
        this.List.Clear();
        this.List.AddRange(this.DefaultList);
    }

    public void AppendDefaultText(StringBuilder stringBuilder, Settings settings, bool isNested)
    {
        if (this.IsRequired)
        {
            stringBuilder.Append(Constants.RequiredText);
            return;
        }

        if (this.defaultValueHelpText != null)
        {
            stringBuilder.Append(Constants.DefaultText);
            stringBuilder.Append(this.defaultValueHelpText);
        }

        stringBuilder.Append(Constants.DefaultText);
        if (!SerializationHelper.SerializeTo(this, this.DefaultList, stringBuilder, settings, null))
        {
            stringBuilder.Append(Constants.NoneText);
        }
    }

    private R<ParserError> DeserializeFrom(ReadOnlySpan<char> value, Settings settings)
    {
        SerializationHelper.DeserializeTo(this.List, this.deserialize, value, settings);
        return R.Success();
    }
}