// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Option.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Options;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Sundew.Base;
using Sundew.CommandLine.Internal.Extensions;
using Sundew.CommandLine.Internal.Helpers;

internal sealed class Option : IOption
{
    private readonly Deserialize deserialize;
    private readonly Serialize serialize;
    private readonly bool useDoubleQuotes;
    private readonly string? defaultValueHelpText;
    private readonly string defaultValue;

    public Option(
        string? name,
        string alias,
        Serialize serialize,
        Deserialize deserialize,
        bool isRequired,
        string helpText,
        bool useDoubleQuotes,
        Separators separators,
        CultureInfo cultureInfo,
        string? defaultValueHelpText,
        int index,
        IArgumentMissingInfo? owner)
    {
        this.Name = name;
        this.Alias = alias;
        this.serialize = serialize;
        this.deserialize = deserialize;
        this.useDoubleQuotes = useDoubleQuotes;
        this.defaultValueHelpText = defaultValueHelpText;
        this.Index = index;
        this.Owner = owner;
        this.IsRequired = isRequired;
        this.HelpLines = HelpTextHelper.GetHelpLines(helpText);
        this.Separators = separators;
        this.Usage = HelpTextHelper.GetUsage(name, alias);
        this.defaultValue = this.serialize(cultureInfo).ToString();
    }

    public string? Name { get; }

    public string Alias { get; }

    public bool IsRequired { get; }

    public int Index { get; }

    public IArgumentMissingInfo? Owner { get; }

    public bool IsNesting => false;

    public bool IsChoice => this.Owner != null;

    public string Usage { get; }

    public IReadOnlyList<string> HelpLines { get; }

    public Separators Separators { get; }

    public R<bool, GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings, bool useAliases)
    {
        var serializedValue = this.SerializeValue(settings);
        if (serializedValue.IsEmpty)
        {
            if (this.IsRequired)
            {
                return R.Error(new GeneratorError(this, GeneratorErrorType.RequiredOptionMissing));
            }

            return R.Success(false);
        }

        var usedAlias = SerializationHelper.AppendNameOrAlias(stringBuilder, this.Name, this.Alias, useAliases);
        stringBuilder.Append(usedAlias ? this.Separators.AliasSeparator : this.Separators.NameSeparator);
        SerializationHelper.AppendQuotes(stringBuilder, this.useDoubleQuotes);
        SerializationHelper.EscapeValuesIfNeeded(stringBuilder, serializedValue);
        stringBuilder.Append(serializedValue);
        SerializationHelper.AppendQuotes(stringBuilder, this.useDoubleQuotes);

        return R.Success(true);
    }

    public RoE<ParserError> DeserializeFrom(
        CommandLineArgumentsParser commandLineArgumentsParser,
        ArgumentList argumentList,
        ReadOnlySpan<char> value,
        Settings settings)
    {
        try
        {
            this.deserialize(value, settings.CultureInfo);
            return R.Success();
        }
        catch (Exception e)
        {
            throw new SerializationException(
                this,
                string.Format(settings.CultureInfo, Constants.ArgumentDeserializationErrorFormat, this.Usage, value.ToString()),
                e);
        }
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
        this.deserialize(this.defaultValue.AsSpan(), cultureInfo);
    }

    public void AppendDefaultText(StringBuilder stringBuilder, Settings settings, bool isNested)
    {
        if (this.IsRequired && !isNested)
        {
            stringBuilder.Append(Constants.RequiredText);
            return;
        }

        if (this.defaultValueHelpText != null)
        {
            stringBuilder.Append(Constants.DefaultText);
            stringBuilder.Append(this.defaultValueHelpText);
            return;
        }

        if (this.defaultValue.Length == 0)
        {
            if (this.IsRequired)
            {
                stringBuilder.Append(Constants.RequiredText);
                return;
            }

            stringBuilder.Append(Constants.DefaultText);
            stringBuilder.Append(Constants.NoneText);
            return;
        }

        stringBuilder.Append(Constants.DefaultText);
        stringBuilder.Append(this.defaultValue);
    }

    private ReadOnlySpan<char> SerializeValue(Settings settings)
    {
        try
        {
            return this.serialize(settings.CultureInfo);
        }
        catch (Exception e)
        {
            throw new SerializationException(
                this,
                string.Format(settings.CultureInfo, Constants.ArgumentSerializationErrorFormat, this.Usage),
                e);
        }
    }
}