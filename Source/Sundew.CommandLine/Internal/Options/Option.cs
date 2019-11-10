// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Option.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Options
{
    using System;
    using System.Text;
    using Sundew.Base.Computation;
    using Sundew.CommandLine.Internal.Extensions;
    using Sundew.CommandLine.Internal.Helpers;

    internal sealed class Option : IOption
    {
        private readonly Deserialize deserialize;
        private readonly Serialize serialize;
        private readonly bool useDoubleQuotes;

        public Option(
            string name,
            string alias,
            Serialize serialize,
            Deserialize deserialize,
            bool isRequired,
            string helpText,
            bool useDoubleQuotes,
            Separators separators)
        {
            this.Name = name;
            this.Alias = alias;
            this.serialize = serialize;
            this.deserialize = deserialize;
            this.useDoubleQuotes = useDoubleQuotes;
            this.IsRequired = isRequired;
            this.HelpText = helpText;
            this.Separators = separators;
            this.Usage = HelpTextHelper.GetUsage(name, alias);
        }

        public string Name { get; }

        public string Alias { get; }

        public bool IsRequired { get; }

        public bool IsNesting => false;

        public string Usage { get; }

        public string HelpText { get; }

        public Separators Separators { get; }

        public Result.IfError<GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings, bool useAliases)
        {
            var serializedValue = this.SerializeValue(settings);
            if (serializedValue.IsEmpty)
            {
                if (this.IsRequired)
                {
                    return Result.Error(new GeneratorError(this, GeneratorErrorType.RequiredOptionMissing));
                }

                return Result.Success();
            }

            var usedAlias = SerializationHelper.AppendNameOrAlias(stringBuilder, this.Name, this.Alias, useAliases);
            stringBuilder.Append(usedAlias ? this.Separators.AliasSeparator : this.Separators.NameSeparator);
            SerializationHelper.AppendQuotes(stringBuilder, this.useDoubleQuotes);
            SerializationHelper.EscapeValuesIfNeeded(stringBuilder, serializedValue);
            stringBuilder.Append(serializedValue);
            SerializationHelper.AppendQuotes(stringBuilder, this.useDoubleQuotes);

            return Result.Success();
        }

        public Result.IfError<ParserError> DeserializeFrom(
            CommandLineArgumentsParser commandLineArgumentsParser,
            ArgumentList argumentList,
            ReadOnlySpan<char> value,
            Settings settings)
        {
            try
            {
                this.deserialize(value, settings.CultureInfo);
                return Result.Success();
            }
            catch (Exception e)
            {
                throw new SerializationException(
                    this,
                    string.Format(settings.CultureInfo, Constants.ArgumentDeserializationErrorFormat, this.Usage, value.ToString()),
                    e);
            }
        }

        public void AppendHelpText(StringBuilder stringBuilder, Settings settings, int maxName, int maxAlias, int maxHelpText, int indent, bool isForVerb)
        {
            HelpTextHelper.AppendHelpText(stringBuilder, settings, this, maxName, maxAlias, maxHelpText, indent, isForVerb);
        }

        public void AppendDefaultText(StringBuilder stringBuilder, Settings settings, bool isNested)
        {
            if (this.IsRequired && !isNested)
            {
                stringBuilder.AppendLine(Constants.RequiredText);
                return;
            }

            var @default = this.SerializeValue(settings);
            if (@default.IsEmpty)
            {
                if (this.IsRequired)
                {
                    stringBuilder.AppendLine(Constants.RequiredText);
                    return;
                }

                stringBuilder.Append(Constants.DefaultText);
                stringBuilder.AppendLine(Constants.NoneText);
                return;
            }

            stringBuilder.Append(Constants.DefaultText);
            stringBuilder.AppendLine(@default);
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
}