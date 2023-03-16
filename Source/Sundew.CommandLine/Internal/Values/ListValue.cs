// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListValue.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Values;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Sundew.Base.Collections;
using Sundew.Base.Primitives.Computation;
using Sundew.Base.Text;
using Sundew.CommandLine.Internal.Helpers;

internal sealed class ListValue<TValue> : IValue, IListSerializationInfo<TValue>
{
    private readonly Deserialize<TValue> deserialize;

    public ListValue(
        string name,
        IList<TValue> list,
        Serialize<TValue> serialize,
        Deserialize<TValue> deserialize,
        bool isRequired,
        string helpText,
        bool useDoubleQuotes,
        string? defaultValueHelpText)
    {
        this.Name = name.Uncapitalize(CultureInfo.InvariantCulture);
        this.List = list;
        this.deserialize = deserialize;
        this.Serialize = serialize;
        this.IsRequired = isRequired;
        this.HelpLines = HelpTextHelper.GetHelpLines(helpText);
        this.UseDoubleQuotes = useDoubleQuotes;
        this.DefaultValueHelpText = defaultValueHelpText;
        this.DefaultList = this.List.ToList();
    }

    public string Name { get; }

    public IList<TValue> List { get; }

    public Serialize<TValue> Serialize { get; }

    public bool IsRequired { get; }

    public bool IsList => true;

    public bool UseDoubleQuotes { get; }

    public string? DefaultValueHelpText { get; }

    public string Usage => $"<{this.Name}>";

    public IReadOnlyList<string> HelpLines { get; }

    public bool IsNesting { get; }

    public List<TValue> DefaultList { get; }

    public void ResetToDefault(CultureInfo cultureInfo)
    {
        this.List.Clear();
        this.List.AddRange(this.DefaultList);
    }

    public R<GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings)
    {
        var wasSerialized = SerializationHelper.SerializeTo(this, this.List, stringBuilder, settings, null);
        if (!wasSerialized && this.IsRequired)
        {
            return R.Error(new GeneratorError(GeneratorErrorType.RequiredValuesMissing));
        }

        return R.Success();
    }

    public R<ParserError> DeserializeFrom(ReadOnlySpan<char> value, ArgumentList argumentList, Settings settings)
    {
        this.List.Clear();
        SerializationHelper.DeserializeTo(this.List, this.deserialize, value, settings);
        if (argumentList.TryMoveNext(out _))
        {
            foreach (var argument in argumentList)
            {
                SerializationHelper.DeserializeTo(this.List, this.deserialize, CommandLineArgumentsParser.RemoveValueEscapeIfNeeded(argument.Span), settings);
            }
        }

        return R.Success();
    }

    public void AppendMissingArgumentsHint(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine(this.Usage);
    }
}