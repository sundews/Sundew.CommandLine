// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentsBuilder.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Sundew.Base.Collections;
    using Sundew.CommandLine.Internal.Options;
    using Sundew.CommandLine.Internal.Values;

    internal sealed class ArgumentsBuilder : IArgumentsBuilder
    {
        private readonly Dictionary<string, IOption> options = new Dictionary<string, IOption>();
        private readonly Dictionary<string, Switch> switches = new Dictionary<string, Switch>();
        private bool isConfigured;

        public ArgumentsBuilder()
        {
            this.Options = new ArgumentRegistry<IOption>(this.options);
            this.Switches = new ArgumentRegistry<Switch>(this.switches);
        }

        public List<IArgumentInfo> RequiredArguments { get; } = new List<IArgumentInfo>();

        public ArgumentRegistry<IOption> Options { get; }

        public ArgumentRegistry<Switch> Switches { get; }

        public ValueRegistry Values { get; } = new ValueRegistry();

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
                null);
            this.AddOption(option, actualSeparator);
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
                helpText);
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
                null);
            this.AddOption(option, default);
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
               defaultValueText);
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
                helpText);
            this.AddOption(option, default);
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
                defaultValueText);
            this.AddOption(option, default);
        }

        public void AddSwitch(string? name, string alias, bool value, Action<bool> setValue, string helpText)
        {
            var isNameNullOrEmpty = string.IsNullOrEmpty(name);
            var isAliasNullOrEmpty = string.IsNullOrEmpty(alias);
            VerifyNameAndAlias(isNameNullOrEmpty, isAliasNullOrEmpty);

            var @switch = new Switch(name, alias, value, setValue, helpText);
            if (!isNameNullOrEmpty)
            {
                this.switches.Add($"-{@switch.Name}", @switch);
            }

            if (!isAliasNullOrEmpty)
            {
                this.switches.Add($"--{@switch.Alias}", @switch);
            }
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

        private void AddOption(IOption option, Separators separators)
        {
            var isNameNullOrEmpty = string.IsNullOrEmpty(option.Name);
            var isAliasNullOrEmpty = string.IsNullOrEmpty(option.Alias);
            VerifyNameAndAlias(isNameNullOrEmpty, isAliasNullOrEmpty);

            if (!isNameNullOrEmpty)
            {
                var isAddingNameSeparator = !char.IsWhiteSpace(separators.NameSeparator);
                this.options.Add($"-{option.Name}{(isAddingNameSeparator ? separators.NameSeparator.ToString() : string.Empty)}", option);
            }

            if (!isAliasNullOrEmpty)
            {
                var isAddingAliasSeparator = !char.IsWhiteSpace(separators.AliasSeparator);
                this.options.Add($"--{option.Alias}{(isAddingAliasSeparator ? separators.AliasSeparator.ToString() : string.Empty)}", option);
            }

            if (option.IsRequired)
            {
                this.RequiredArguments.Add(option);
            }
        }

        private Separators GetActualSeparator(Separators separators)
        {
            var actualSeparator = this.Separators;
            if (!separators.IsDefault)
            {
                actualSeparator = separators;
            }

            return actualSeparator;
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
}