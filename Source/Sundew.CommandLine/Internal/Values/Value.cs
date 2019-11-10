// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Value.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Values
{
    using System;
    using System.Globalization;
    using System.Text;
    using Sundew.Base.Computation;
    using Sundew.Base.Text;
    using Sundew.CommandLine.Internal.Extensions;
    using Sundew.CommandLine.Internal.Helpers;

    internal class Value : IValue
    {
        private readonly Serialize serialize;
        private readonly Deserialize deserialize;
        private readonly bool useDoubleQuotes;
        private bool hasBeenSet = false;

        public Value(string name, Serialize serialize, Deserialize deserialize, bool isRequired, string helpText, bool useDoubleQuotes)
        {
            this.serialize = serialize;
            this.deserialize = deserialize;
            this.Name = name.Uncapitalize(CultureInfo.InvariantCulture);
            this.HelpText = helpText;
            this.useDoubleQuotes = useDoubleQuotes;
            this.IsRequired = isRequired;
        }

        public bool IsRequired { get; }

        public bool IsList => false;

        public string Usage => $"<{this.Name}>";

        public string Name { get; }

        public string HelpText { get; }

        public bool IsNesting { get; } = false;

        public Result.IfError<ParserError> DeserializeFrom(ReadOnlySpan<char> argument, Settings settings)
        {
            if (this.hasBeenSet)
            {
                return Result.Error(new ParserError(ParserErrorType.OnlySingleValueAllowed, Constants.OnlyASingleValueIsAllowedErrorText));
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
            return Result.Success();
        }

        public Result.IfError<GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings)
        {
            var serializedValue = this.SerializeValue(settings);
            if (serializedValue.IsEmpty)
            {
                if (this.IsRequired)
                {
                    return Result.Error(new GeneratorError(GeneratorErrorType.RequiredValuesMissing));
                }

                return Result.Success();
            }

            SerializationHelper.AppendQuotes(stringBuilder, this.useDoubleQuotes);
            SerializationHelper.EscapeValuesIfNeeded(stringBuilder, serializedValue);
            stringBuilder.Append(serializedValue);
            SerializationHelper.AppendQuotes(stringBuilder, this.useDoubleQuotes);

            return Result.Success();
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
}