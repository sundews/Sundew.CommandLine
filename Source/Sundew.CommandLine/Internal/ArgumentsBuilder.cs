// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentsBuilder.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sundew.Base.Collections;
using Sundew.Base.Equality;
using Sundew.CommandLine.Internal.Enums;
using Sundew.CommandLine.Internal.Options;
using Sundew.CommandLine.Internal.Values;

internal sealed class ArgumentsBuilder : IArgumentsBuilder
{
    private readonly Dictionary<ReadOnlyMemory<char>, IOption> options = new(ReadOnlyMemoryCharEqualityComparer.Instance);
    private readonly Dictionary<ReadOnlyMemory<char>, Switch> switches = new(ReadOnlyMemoryCharEqualityComparer.Instance);
    private readonly List<IArgumentHelpInfo> helpOptions;
    private bool isConfigured;
    private int currentOptionIndex;

    public ArgumentsBuilder()
    {
        this.Options = new ArgumentRegistry<IOption>(this.options);
        this.helpOptions = new List<IArgumentHelpInfo>();
        this.Switches = new ArgumentRegistry<Switch>(this.switches);
    }

    public List<IArgumentMissingInfo> RequiredArguments { get; } = new();

    public ArgumentRegistry<IOption> Options { get; }

    public IReadOnlyList<IArgumentHelpInfo> HelpOptions => this.helpOptions;

    public ArgumentRegistry<Switch> Switches { get; }

    public ValueRegistry Values { get; } = new();

    public Separators Separators { get; set; } = Separators.Create();

    public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;

    public void AddRequired(string? name, string alias, Func<string> serialize, Action<string> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default)
    {
        this.AddRequired(name, alias, (ci) => serialize().AsSpan(), (x, ci) => deserialize(x.ToString()), helpText, useDoubleQuotes, separators);
    }

    public void AddRequired(string? name, string alias, Func<CultureInfo, string> serialize, Action<string, CultureInfo> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default)
    {
        this.AddRequired(name, alias, (ci) => serialize(ci).AsSpan(), (x, ci) => deserialize(x.ToString(), ci), helpText, useDoubleQuotes, separators);
    }

    public void AddRequired(string? name, string alias, Serialize serialize, Deserialize deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default)
    {
        var actualSeparator = this.GetActualSeparator(separators);
        var option = new Option(
            name,
            alias,
            serialize,
            deserialize,
            true,
            helpText,
            useDoubleQuotes,
            actualSeparator,
            this.CultureInfo,
            null,
            this.GetIndexAndIncrement(),
            null);
        this.AddOption(option, actualSeparator);
    }

    public void AddRequiredEnum<TEnum>(string? name, string alias, Func<TEnum> getEnumFunc, Action<TEnum> setEnumAction, string helpText, Separators separators = default, string? defaultValueText = null)
        where TEnum : Enum
    {
        this.AddRequiredEnum(name, alias, getEnumFunc, setEnumAction, helpText, separators, defaultValueText);
    }

    public void AddRequiredEnum<TEnum>(string? name, string alias, Func<TEnum> getEnumFunc, Action<TEnum> setEnumAction, IEnumerable<TEnum> enumOptions, string helpText, Separators separators = default, string? defaultValueText = null)
        where TEnum : Enum
    {
        var enumSerializer = new EnumSerializer<TEnum>(enumOptions);
        var actualSeparator = this.GetActualSeparator(separators);
        this.AddOption(
            new Option(
                name,
                alias,
                _ => enumSerializer.Serialize(getEnumFunc()),
                (value, _) => setEnumAction(enumSerializer.Deserialize(value)),
                true,
                string.Format(helpText, enumSerializer.GetAllValues()),
                false,
                actualSeparator,
                this.CultureInfo,
                defaultValueText,
                this.GetIndexAndIncrement(),
                null),
            actualSeparator);
    }

    public void AddRequired<TOptions>(string? name, string alias, TOptions? options, Func<TOptions> getDefault, Action<TOptions> setOptions, string helpText)
        where TOptions : class, IArguments
    {
        var option = new NestingOption<TOptions>(
            name,
            alias,
            options,
            getDefault,
            setOptions,
            true,
            helpText,
            this.GetIndexAndIncrement(),
            null);
        this.AddOption(option, default);
    }

    public void AddRequiredList(string? name, string alias, IList<string> list, string helpText, bool useDoubleQuotes = false)
    {
        this.AddRequiredList(name, alias, list, (x, ci) => x, (x, ci) => x, helpText, useDoubleQuotes);
    }

    public void AddRequiredList<TValue>(string? name, string alias, IList<TValue> list, Func<TValue, CultureInfo, string> serialize, Func<string, CultureInfo, TValue> deserialize, string helpText, bool useDoubleQuotes = false)
    {
        this.AddRequiredList(name, alias, list, (x, ci) => serialize(x, ci).AsSpan(), (x, ci) => deserialize(x.ToString(), ci), helpText, useDoubleQuotes);
    }

    public void AddRequiredList<TValue>(string? name, string alias, IList<TValue> list, Serialize<TValue> serialize, Deserialize<TValue> deserialize, string helpText, bool useDoubleQuotes = false)
    {
        var option = new ListOption<TValue>(
            name,
            alias,
            list,
            serialize,
            deserialize,
            true,
            helpText,
            useDoubleQuotes,
            null,
            this.GetIndexAndIncrement(),
            null);
        this.AddOption(option, default);
    }

    public void RequireAnyOf(string name, Action<IChoiceBuilder> selectChoiceAction)
    {
        var choiceArgumentInfos = new List<IArgumentInfo>();
        var requiredChoiceArgumentInfo = new RequiredChoiceArgumentInfo(name, choiceArgumentInfos, this.GetIndexAndIncrement());
        this.helpOptions.Add(requiredChoiceArgumentInfo);
        this.RequiredArguments.Add(requiredChoiceArgumentInfo);
        selectChoiceAction(new ChoiceBuilder(this, requiredChoiceArgumentInfo, choiceArgumentInfos));
    }

    public void AddOptional(string? name, string alias, Func<string?> serialize, Action<string> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default, string? defaultValueText = null)
    {
        this.AddOptional(name, alias, (ci) => serialize().AsSpan(), (x, ci) => deserialize(x.ToString()), helpText, useDoubleQuotes, separators, defaultValueText);
    }

    public void AddOptional(string? name, string alias, Func<CultureInfo, string?> serialize, Action<string, CultureInfo> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default, string? defaultValueText = null)
    {
        this.AddOptional(name, alias, (ci) => serialize(ci).AsSpan(), (x, ci) => deserialize(x.ToString(), ci), helpText, useDoubleQuotes, separators, defaultValueText);
    }

    public void AddOptional(string? name, string alias, Serialize serialize, Deserialize deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default, string? defaultValueText = null)
    {
        var actualSeparator = this.GetActualSeparator(separators);
        var option = new Option(
            name,
            alias,
            serialize,
            deserialize,
            false,
            helpText,
            useDoubleQuotes,
            actualSeparator,
            this.CultureInfo,
            defaultValueText,
            this.GetIndexAndIncrement(),
            null);
        this.AddOption(option, actualSeparator);
    }

    public void AddOptional<TOptions>(string? name, string alias, TOptions? options, Func<TOptions> getDefault, Action<TOptions> setOptions, string helpText, string? defaultValueText = null)
        where TOptions : class, IArguments
    {
        var option = new NestingOption<TOptions>(
            name,
            alias,
            options,
            getDefault,
            setOptions,
            false,
            helpText,
            this.GetIndexAndIncrement(),
            null);
        this.AddOption(option, default);
    }

    public void AddOptionalEnum<TEnum>(string? name, string alias, Func<TEnum> getEnumFunc, Action<TEnum> setEnumAction, string helpText, Separators separators = default, string? defaultValueText = null)
        where TEnum : Enum
    {
        this.AddOptionalEnum(name, alias, getEnumFunc, setEnumAction, Enum.GetValues(typeof(TEnum)).Cast<TEnum>(), helpText, separators, defaultValueText);
    }

    public void AddOptionalEnum<TEnum>(string? name, string alias, Func<TEnum> getEnumFunc, Action<TEnum> setEnumAction, IEnumerable<TEnum> enumOptions, string helpText, Separators separators = default, string? defaultValueText = null)
        where TEnum : Enum
    {
        var enumSerializer = new EnumSerializer<TEnum>(enumOptions);
        var actualSeparator = this.GetActualSeparator(separators);
        this.AddOption(
            new Option(
                name,
                alias,
                _ => enumSerializer.Serialize(getEnumFunc()),
                (value, _) => setEnumAction(enumSerializer.Deserialize(value)),
                false,
                string.Format(helpText, enumSerializer.GetAllValues()),
                false,
                actualSeparator,
                this.CultureInfo,
                defaultValueText,
                this.GetIndexAndIncrement(),
                null),
            actualSeparator);
    }

    public void AddOptionalList(string? name, string alias, IList<string> list, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
    {
        this.AddOptionalList(name, alias, list, (x, ci) => x, (x, ci) => x, helpText, useDoubleQuotes, defaultValueText);
    }

    public void AddOptionalList<TValue>(string? name, string alias, IList<TValue> list, Func<TValue, CultureInfo, string> serialize, Func<string, CultureInfo, TValue> deserialize, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
    {
        this.AddOptionalList(name, alias, list, (x, ci) => serialize(x, ci).AsSpan(), (x, ci) => deserialize(x.ToString(), ci), helpText, useDoubleQuotes, defaultValueText);
    }

    public void AddOptionalList<TValue>(string? name, string alias, IList<TValue> list, Serialize<TValue> serialize, Deserialize<TValue> deserialize, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
    {
        var option = new ListOption<TValue>(
            name,
            alias,
            list,
            serialize,
            deserialize,
            false,
            helpText,
            useDoubleQuotes,
            defaultValueText,
            this.GetIndexAndIncrement(),
            null);
        this.AddOption(option, default);
    }

    public void AddSwitch(string? name, string alias, bool value, Action<bool> setValue, string helpText)
    {
        this.AddSwitch(this.CreateSwitch(name, alias, value, setValue, helpText, this.GetIndexAndIncrement(), null));
    }

    public void AddRequiredValue(string name, Func<string> serialize, Action<string> deserialize, string helpText, bool useDoubleQuotes = false)
    {
        this.AddRequiredValue(name, (ci) => serialize().AsSpan(), (x, ci) => deserialize(x.ToString()), helpText, useDoubleQuotes);
    }

    public void AddRequiredValue(string name, Func<CultureInfo, string> serialize, Action<string, CultureInfo> deserialize, string helpText, bool useDoubleQuotes = false)
    {
        this.AddRequiredValue(name, (ci) => serialize(ci).AsSpan(), (x, ci) => deserialize(x.ToString(), ci), helpText, useDoubleQuotes);
    }

    public void AddRequiredValue(string name, Serialize serialize, Deserialize deserialize, string helpText, bool useDoubleQuotes = false)
    {
        this.AddValue(new Value(name, serialize, deserialize, true, helpText, useDoubleQuotes, this.CultureInfo, null));
    }

    public void AddOptionalValue(string name, Func<string?> serialize, Action<string> deserialize, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
    {
        this.AddOptionalValue(name, (ci) => serialize().AsSpan(), (x, ci) => deserialize(x.ToString()), helpText, useDoubleQuotes, defaultValueText);
    }

    public void AddOptionalValue(string name, Func<CultureInfo, string> serialize, Action<string, CultureInfo> deserialize, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
    {
        this.AddOptionalValue(name, (ci) => serialize(ci).AsSpan(), (x, ci) => deserialize(x.ToString(), ci), helpText, useDoubleQuotes, defaultValueText);
    }

    public void AddOptionalValue(string name, Serialize serialize, Deserialize deserialize, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
    {
        this.AddValue(new Value(name, serialize, deserialize, false, helpText, useDoubleQuotes, this.CultureInfo, defaultValueText));
    }

    public void AddRequiredValues<TValue>(string name, IList<TValue> values, Func<TValue, CultureInfo, string> serialize, Func<string, CultureInfo, TValue> deserialize, string helpText, bool useDoubleQuotes = false)
    {
        this.AddRequiredValues(name, values, (x, ci) => serialize(x, ci).AsSpan(), (x, ci) => deserialize(x.ToString(), ci), helpText, useDoubleQuotes);
    }

    public void AddRequiredValues<TValue>(string name, IList<TValue> values, Serialize<TValue> serialize, Deserialize<TValue> deserialize, string helpText, bool useDoubleQuotes = false)
    {
        this.AddValue(new ListValue<TValue>(
            name,
            values,
            serialize,
            deserialize,
            true,
            helpText,
            useDoubleQuotes,
            null));
    }

    public void AddRequiredValues(string name, IList<string> values, string helpText, bool useDoubleQuotes = false)
    {
        this.AddRequiredValues(name, values, (value, ci) => value, (value, ci) => value, helpText, useDoubleQuotes);
    }

    public void AddOptionalValues<TValue>(string name, IList<TValue> values, Func<TValue, CultureInfo, string> serialize, Func<string, CultureInfo, TValue> deserialize, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
    {
        this.AddOptionalValues(name, values, (x, ci) => serialize(x, ci).AsSpan(), (x, ci) => deserialize(x.ToString(), ci), helpText, useDoubleQuotes, defaultValueText);
    }

    public void AddOptionalValues<TValue>(string name, IList<TValue> values, Serialize<TValue> serialize, Deserialize<TValue> deserialize, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
    {
        this.AddValue(new ListValue<TValue>(
            name,
            values,
            serialize,
            deserialize,
            false,
            helpText,
            useDoubleQuotes,
            defaultValueText));
    }

    public void AddOptionalValues(string name, IList<string> values, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
    {
        this.AddOptionalValues(name, values, (value, ci) => value, (value, ci) => value, helpText, useDoubleQuotes, defaultValueText);
    }

    public void PrepareBuilder(IArguments arguments, bool allowReset)
    {
        if (!this.isConfigured)
        {
            arguments.Configure(this);
            this.isConfigured = true;
        }
        else if (allowReset)
        {
            this.ResetToDefault();
        }
    }

    internal void AddOption(IOption option, Separators separators)
    {
        var isNameNullOrEmpty = string.IsNullOrEmpty(option.Name);
        var isAliasNullOrEmpty = string.IsNullOrEmpty(option.Alias);
        VerifyNameAndAlias(isNameNullOrEmpty, isAliasNullOrEmpty);

        if (!isNameNullOrEmpty)
        {
            var isAddingNameSeparator = !char.IsWhiteSpace(separators.NameSeparator);
            this.options.Add($"-{option.Name}{(isAddingNameSeparator ? separators.NameSeparator.ToString() : string.Empty)}".AsMemory(), option);
        }

        if (!isAliasNullOrEmpty)
        {
            var isAddingAliasSeparator = !char.IsWhiteSpace(separators.AliasSeparator);
            this.options.Add($"--{option.Alias}{(isAddingAliasSeparator ? separators.AliasSeparator.ToString() : string.Empty)}".AsMemory(), option);
        }

        this.helpOptions.Add(option);
        if (option.IsRequired)
        {
            this.RequiredArguments.Add(option);
        }
    }

    internal Switch CreateSwitch(string? name, string alias, bool value, Action<bool> setValue, string helpText, int switchIndex, IArgumentMissingInfo? owner)
    {
        var isNameNullOrEmpty = string.IsNullOrEmpty(name);
        var isAliasNullOrEmpty = string.IsNullOrEmpty(alias);
        VerifyNameAndAlias(isNameNullOrEmpty, isAliasNullOrEmpty);
        return new(name, alias, value, setValue, helpText, switchIndex, owner);
    }

    internal void AddSwitch(Switch @switch)
    {
        if (!string.IsNullOrEmpty(@switch.Name))
        {
            this.switches.Add($"-{@switch.Name}".AsMemory(), @switch);
        }

        if (!string.IsNullOrEmpty(@switch.Alias))
        {
            this.switches.Add($"--{@switch.Alias}".AsMemory(), @switch);
        }

        this.helpOptions.Add(@switch);
    }

    internal Separators GetActualSeparator(Separators separators)
    {
        var actualSeparator = this.Separators;
        if (!separators.IsDefault)
        {
            actualSeparator = separators;
        }

        return actualSeparator;
    }

    internal int GetIndexAndIncrement()
    {
        return this.currentOptionIndex++;
    }

    private static void VerifyNameAndAlias(bool isNameNullOrEmpty, bool isAliasNullOrEmpty)
    {
        if (isNameNullOrEmpty && isAliasNullOrEmpty)
        {
            throw new NotSupportedException("Both name and alias cannot be null or empty.");
        }
    }

    private void ResetToDefault()
    {
        this.options.Values.Distinct().ForEach(x => x.ResetToDefault(this.CultureInfo));
        this.switches.Values.Distinct().ForEach(x => x.ResetToDefault(this.CultureInfo));
        this.Values.ResetToDefault(this.CultureInfo);
    }

    private void AddValue(IValue value)
    {
        this.Values.Add(value);
        if (value.IsRequired)
        {
            this.RequiredArguments.Add(value);
        }
    }
}