// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueRegistry.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Values;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Sundew.Base;
using Sundew.CommandLine.Internal.Helpers;

internal class ValueRegistry : IEnumerable<IValue>
{
    private const string CannotAddARequiredValueAfterAnOptionalValueText = "Cannot add a required value after an optional value.";
    private const string CannotAddAnythingAfterAListOfValuesText = "Cannot add anything after a list of values.";
    private readonly List<IValue> values = new();

    public bool HasValues => this.values.Any();

    public void Add(IValue value)
    {
        var lastValue = this.values.LastOrDefault();
        if (lastValue != null)
        {
            if (!lastValue.IsRequired && value.IsRequired)
            {
                throw new ArgumentsBuilderException(CannotAddARequiredValueAfterAnOptionalValueText);
            }

            if (lastValue.IsList)
            {
                throw new ArgumentsBuilderException(CannotAddAnythingAfterAListOfValuesText);
            }
        }

        this.values.Add(value);
    }

    public R<ParserError> DeserializeFrom(ArgumentsBuilder argumentsBuilder, ArgumentList argumentList, Settings settings)
    {
        R<ParserError> result = R.Success();
        var valueIndex = 0;
        foreach (var argument in argumentList)
        {
            var value = this.values[valueIndex];
            result = value.DeserializeFrom(CommandLineArgumentsParser.RemoveValueEscapeIfNeeded(argument.Span), argumentList, settings);
            if (!result)
            {
                return result;
            }

            argumentsBuilder.RequiredArguments.Remove(value);
            if (!value.IsList)
            {
                valueIndex++;
            }
        }

        return result;
    }

    public R<GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings)
    {
        R<GeneratorError> result = R.Success();
        foreach (var value in this.values)
        {
            result = value.SerializeTo(stringBuilder, settings);
            if (!result)
            {
                return result;
            }

            stringBuilder.Append(Constants.SpaceCharacter);
        }

        return result;
    }

    public void AppendHelpText(StringBuilder stringBuilder, TextSizes textSizes, int indent, bool isForVerb, Settings settings)
    {
        foreach (var value in this.values)
        {
            HelpTextHelper.AppendHelpText(stringBuilder, value, textSizes, indent, isForVerb, settings);
        }
    }

    public IEnumerator<IValue> GetEnumerator()
    {
        return this.values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public void ResetToDefault(CultureInfo cultureInfo)
    {
        this.values.ForEach(x => x.ResetToDefault(cultureInfo));
    }
}