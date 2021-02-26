// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NestingOption.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Options
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Sundew.Base.Computation;
    using Sundew.CommandLine.Internal.Helpers;

    internal class NestingOption<TOptions> : IOption
        where TOptions : class, IArguments
    {
        private readonly TOptions? options;
        private readonly Func<TOptions> getDefault;
        private readonly Action<TOptions> setOptions;
        private readonly ArgumentsBuilder argumentsBuilder = new();

        public NestingOption(
            string? name,
            string alias,
            TOptions? options,
            Func<TOptions> getDefault,
            Action<TOptions> setOptions,
            bool isRequired,
            string helpText,
            int index)
        {
            this.options = options;
            this.getDefault = getDefault;
            this.setOptions = setOptions;
            this.Name = name;
            this.Alias = alias;
            this.IsRequired = isRequired;
            this.Index = index;
            this.HelpLines = HelpTextHelper.GetHelpLines(helpText);
            this.Usage = HelpTextHelper.GetUsage(name, alias);
        }

        public string? Name { get; }

        public string Alias { get; }

        public Separators Separators => default;

        public bool IsRequired { get; }

        public int Index { get; }

        public bool IsNesting => true;

        public string Usage { get; }

        public IReadOnlyList<string> HelpLines { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "It's the proposed way of handling missing cases for enum switches.")]
        public Result<bool, GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings, bool useAliases)
        {
            var actualOption = this.options == null && this.IsRequired ? this.getDefault() : this.options;
            if (actualOption != null)
            {
                SerializationHelper.AppendNameOrAlias(stringBuilder, this.Name, this.Alias, useAliases);
                stringBuilder.Append(Constants.SpaceCharacter);
                var result = SerializeValue(actualOption, stringBuilder, settings, useAliases);

                if (result)
                {
                    return result.WithValue(true);
                }

                var error = result.Error;
                switch (error.Type)
                {
                    case GeneratorErrorType.SerializationException:
                        throw new SerializationException(
                            this,
                            string.Format(settings.CultureInfo, Constants.NestedArgumentSerializationFormat, this.Usage, this.options, error.SerializationException),
                            error.SerializationException!);
                    case GeneratorErrorType.RequiredOptionMissing:
                    case GeneratorErrorType.InnerGeneratorError:
                        return result.Convert(false, innerGeneratorError => new GeneratorError(this, innerGeneratorError));
                    default:
                        throw new ArgumentOutOfRangeException(nameof(error.Type), error.Type, string.Format(settings.CultureInfo, Constants.ErrorTypeNotSupportedFormat, error.Type));
                }
            }

            return Result.Success(false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "It's the proposed way of handling missing cases for enum switches.")]
        public Result.IfError<ParserError> DeserializeFrom(
            CommandLineArgumentsParser commandLineArgumentsParser,
            ArgumentList argumentList,
            ReadOnlySpan<char> value,
            Settings settings)
        {
            var options = this.getDefault();
            var result = this.DeserializeValue(commandLineArgumentsParser, argumentList, options, settings);
            if (result)
            {
                this.setOptions(options);
                return result;
            }

            var error = result.Error;
            var argumentText = value.ToString();
            switch (error.Type)
            {
                case ParserErrorType.Info:
                    return result.ConvertError(innerParserError => new ParserError(innerParserError, string.Format(settings.CultureInfo, Constants.InnerInfoErrorFormat, this.Usage, argumentText)));
                case ParserErrorType.InnerParserError:
                    return result.ConvertError(innerParserError => new ParserError(innerParserError, string.Format(settings.CultureInfo, Constants.InnerParserErrorFormat, this.Usage, argumentText)));
                case ParserErrorType.RequiredArgumentMissing:
                    return result.ConvertError(innerParserError => new ParserError(innerParserError, string.Format(settings.CultureInfo, Constants.InnerRequiredErrorFormat, this.Usage, argumentText)));
                case ParserErrorType.OptionArgumentMissing:
                    return result.ConvertError(innerParserError => new ParserError(innerParserError, string.Format(settings.CultureInfo, Constants.InnerOptionArgumentErrorFormat, this.Usage, argumentText)));
                case ParserErrorType.UnknownOption:
                    return result.ConvertError(innerParserError => new ParserError(innerParserError, string.Format(settings.CultureInfo, Constants.InnerInvalidOptionFormat, this.Usage, argumentText)));
                case ParserErrorType.OnlySingleValueAllowed:
                    return result;
                case ParserErrorType.HelpRequested:
                    return result;
                case ParserErrorType.UnknownVerb:
                    return result;
                case ParserErrorType.InvalidStartIndex:
                    return result;
                case ParserErrorType.ArgumentsAndVerbsAreNotConfigured:
                    return result;
                case ParserErrorType.ArgumentsNotConfiguredOrUnknownVerb:
                    return result;
                case ParserErrorType.SerializationException:
                    var message = string.Format(settings.CultureInfo, Constants.NestedArgumentDeserializationFormat, this.Usage, argumentText, error.SerializationException);
                    throw new SerializationException(
                        this,
                        message,
                        error.SerializationException!);
                default:
                    throw new ArgumentOutOfRangeException(nameof(error.Type), error.Type, string.Format(settings.CultureInfo, Constants.ErrorTypeNotSupportedFormat, error.Type));
            }
        }

        public void AppendHelpText(StringBuilder stringBuilder, Settings settings, int maxName, int maxAlias, int maxHelpText, int indent, bool isForVerb)
        {
            HelpTextHelper.AppendHelpText(stringBuilder, settings, this, maxName, maxAlias, maxHelpText, indent, false);
            var argumentsBuilder = new ArgumentsBuilder { Separators = settings.Separators };
            var defaultOptions = this.options ?? this.getDefault();
            defaultOptions.Configure(argumentsBuilder);
            CommandLineHelpGenerator.AppendCommandLineHelpText(argumentsBuilder, stringBuilder, indent + 1, maxName, maxAlias, isForVerb, settings);
        }

        public void ResetToDefault(CultureInfo cultureInfo)
        {
            this.setOptions(this.getDefault());
        }

        public void AppendDefaultText(StringBuilder stringBuilder, Settings settings, bool isNested)
        {
            if (this.IsRequired)
            {
                stringBuilder.Append(Constants.RequiredText);
                return;
            }

            if (this.options == null)
            {
                stringBuilder.Append(Constants.DefaultText + Constants.NoneText);
                return;
            }

            stringBuilder.Append(Constants.DefaultText + Constants.SeeBelowText);
        }

        private static Result.IfError<GeneratorError> SerializeValue(TOptions options, StringBuilder stringBuilder, Settings settings, bool useAliases)
        {
            return CommandLineArgumentsGenerator.Generate(options, stringBuilder, settings, useAliases);
        }

        private Result.IfError<ParserError> DeserializeValue(CommandLineArgumentsParser commandLineArgumentsParser, ArgumentList argumentList, TOptions options, Settings settings)
        {
            this.argumentsBuilder.PrepareBuilder(options, true);
            return commandLineArgumentsParser.Parse(this.argumentsBuilder, settings, argumentList);
        }
    }
}