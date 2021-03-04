// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChoiceBuilder.cs" company="Hukano">
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
    using Sundew.CommandLine.Internal.Enums;
    using Sundew.CommandLine.Internal.Options;

    internal class ChoiceBuilder : IChoiceBuilder
    {
        private readonly ArgumentsBuilder argumentsBuilder;
        private readonly RequiredChoiceArgumentInfo requiredChoiceArgumentInfo;
        private readonly List<IOption> choiceOptions;

        public ChoiceBuilder(ArgumentsBuilder argumentsBuilder, RequiredChoiceArgumentInfo requiredChoiceArgumentInfo, List<IOption> choiceOptions)
        {
            this.argumentsBuilder = argumentsBuilder;
            this.requiredChoiceArgumentInfo = requiredChoiceArgumentInfo;
            this.choiceOptions = choiceOptions;
        }

        public IChoiceBuilder Add(string? name, string alias, Func<string?> serialize, Action<string> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default, string? defaultValueText = null)
        {
            return this.Add(name, alias, (ci) => serialize().AsSpan(), (x, ci) => deserialize(x.ToString()), helpText, useDoubleQuotes, separators, defaultValueText);
        }

        public IChoiceBuilder Add(string? name, string alias, Func<CultureInfo, string?> serialize, Action<string, CultureInfo> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default, string? defaultValueText = null)
        {
            return this.Add(name, alias, (ci) => serialize(ci).AsSpan(), (x, ci) => deserialize(x.ToString(), ci), helpText, useDoubleQuotes, separators, defaultValueText);
        }

        public IChoiceBuilder Add(string? name, string alias, Serialize serialize, Deserialize deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default, string? defaultValueText = null)
        {
            var actualSeparator = this.argumentsBuilder.GetActualSeparator(separators);
            var option = new Option(
                name,
                alias,
                serialize,
                deserialize,
                false,
                helpText,
                useDoubleQuotes,
                actualSeparator,
                this.argumentsBuilder.CultureInfo,
                defaultValueText,
                this.argumentsBuilder.GetIndex(),
                this.requiredChoiceArgumentInfo);
            this.argumentsBuilder.AddOption(option, actualSeparator);
            this.choiceOptions.Add(option);
            return this;
        }

        public IChoiceBuilder Add<TOptions>(string? name, string alias, TOptions? options, Func<TOptions> getDefault, Action<TOptions> setOptions, string helpText, string? defaultValueText = null)
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
                this.argumentsBuilder.GetIndex(),
                this.requiredChoiceArgumentInfo);
            this.argumentsBuilder.AddOption(option, default);
            this.choiceOptions.Add(option);
            return this;
        }

        public IChoiceBuilder AddEnum<TEnum>(string? name, string alias, Func<TEnum> getEnumFunc, Action<TEnum> setEnumAction, string helpText, Separators separators = default, string? defaultValueText = null)
            where TEnum : Enum
        {
            return this.AddEnum(name, alias, getEnumFunc, setEnumAction, Enum.GetValues(typeof(TEnum)).Cast<TEnum>(), helpText, separators, defaultValueText);
        }

        public IChoiceBuilder AddEnum<TEnum>(string? name, string alias, Func<TEnum> getEnumFunc, Action<TEnum> setEnumAction, IEnumerable<TEnum> enumOptions, string helpText, Separators separators = default, string? defaultValueText = null)
            where TEnum : Enum
        {
            var enumSerializer = new EnumSerializer<TEnum>(enumOptions);
            var actualSeparator = this.argumentsBuilder.GetActualSeparator(separators);
            var option = new Option(
                name,
                alias,
                _ => enumSerializer.Serialize(getEnumFunc()),
                (value, _) => setEnumAction(enumSerializer.Deserialize(value)),
                false,
                string.Format(helpText, enumSerializer.GetAllValues()),
                false,
                actualSeparator,
                this.argumentsBuilder.CultureInfo,
                defaultValueText,
                this.argumentsBuilder.GetIndex(),
                null);
            this.argumentsBuilder.AddOption(option, actualSeparator);
            this.choiceOptions.Add(option);
            return this;
        }

        public IChoiceBuilder AddList(string? name, string alias, IList<string> list, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
        {
            return this.AddList(name, alias, list, (x, ci) => x, (x, ci) => x, helpText, useDoubleQuotes, defaultValueText);
        }

        public IChoiceBuilder AddList<TValue>(string? name, string alias, IList<TValue> list, Func<TValue, CultureInfo, string> serialize, Func<string, CultureInfo, TValue> deserialize, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
        {
            return this.AddList(name, alias, list, (x, ci) => serialize(x, ci).AsSpan(), (x, ci) => deserialize(x.ToString(), ci), helpText, useDoubleQuotes, defaultValueText);
        }

        public IChoiceBuilder AddList<TValue>(string? name, string alias, IList<TValue> list, Serialize<TValue> serialize, Deserialize<TValue> deserialize, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null)
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
                this.argumentsBuilder.GetIndex(),
                this.requiredChoiceArgumentInfo);
            this.argumentsBuilder.AddOption(option, default);
            this.choiceOptions.Add(option);
            return this;
        }
    }
}