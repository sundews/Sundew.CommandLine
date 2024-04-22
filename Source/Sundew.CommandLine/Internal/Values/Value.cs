// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Value.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Values;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Sundew.Base;
using Sundew.Base.Text;
using Sundew.CommandLine.Internal.Extensions;
using Sundew.CommandLine.Internal.Helpers;

internal class Value : IValue
{
    private readonly Serialize serialize;
    private readonly Deserialize deserialize;
    private readonly bool useDoubleQuotes;
    private readonly string defaultValue;
    private bool hasBeenSet;

    public Value(
        string name,
        Serialize serialize,
        Deserialize deserialize,
        bool isRequired,
        string helpText,
        bool useDoubleQuotes,
        CultureInfo cultureInfo,
        string? defaultValueHelpText)
    {
        this.serialize = serialize;
        this.deserialize = deserialize;
        this.Name = name.Uncapitalize(cultureInfo);
        this.HelpLines = HelpTextHelper.GetHelpLines(helpText);
        this.DefaultValueHelpText = defaultValueHelpText;
        this.useDoubleQuotes = useDoubleQuotes;
        this.IsRequired = isRequired;
        this.defaultValue = this.serialize(cultureInfo).ToString();
    }

    public bool IsRequired { get; }

    public bool IsList => false;

    public string Usage => $"<{this.Name}>";

    public string Name { get; }

    public IReadOnlyList<string> HelpLines { get; }

    public string? DefaultValueHelpText { get; }

    public void ResetToDefault(CultureInfo cultureInfo)
    {
        this.deserialize(this.defaultValue.AsSpan(), cultureInfo);
    }

    public RwE<ParserError> DeserializeFrom(ReadOnlySpan<char> argument, ArgumentList argumentList, Settings settings)
    {
        if (this.hasBeenSet)
        {
            return R.Error(new ParserError(ParserErrorType.OnlySingleValueAllowed, Constants.OnlyASingleValueIsAllowedErrorText));
        }

        try
        {
            this.deserialize(argument, settings.CultureInfo);
        }
        catch (Exception e)
        {
            throw new SerializationException(null, string.Format(settings.CultureInfo, Constants.ListDeserializationErrorFormat, argument.ToString()), e);
        }

        this.hasBeenSet = true;
        return R.Success();
    }

    public RwE<GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings)
    {
        var serializedValue = this.SerializeValue(settings);
        if (serializedValue.IsEmpty)
        {
            if (this.IsRequired)
            {
                return R.Error(new GeneratorError(GeneratorErrorType.RequiredValuesMissing));
            }

            return R.Success();
        }

        SerializationHelper.AppendQuotes(stringBuilder, this.useDoubleQuotes);
        SerializationHelper.EscapeValuesIfNeeded(stringBuilder, serializedValue);
        stringBuilder.Append(serializedValue);
        SerializationHelper.AppendQuotes(stringBuilder, this.useDoubleQuotes);

        return R.Success();
    }

    public void AppendMissingArgumentsHint(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine(this.Usage);
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
                string.Format(settings.CultureInfo, Constants.ValueSerializationErrorText),
                e);
        }
    }
}