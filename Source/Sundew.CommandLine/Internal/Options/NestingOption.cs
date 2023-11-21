// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NestingOption.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Options;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Sundew.Base.Primitives.Computation;
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
        int index,
        IArgumentMissingInfo? owner)
    {
        this.options = options;
        this.getDefault = getDefault;
        this.setOptions = setOptions;
        this.Name = name;
        this.Alias = alias;
        this.IsRequired = isRequired;
        this.Index = index;
        this.Owner = owner;
        this.HelpLines = HelpTextHelper.GetHelpLines(helpText);
        this.Usage = HelpTextHelper.GetUsage(name, alias);
    }

    public string? Name { get; }

    public string Alias { get; }

    public Separators Separators => default;

    public bool IsRequired { get; }

    public int Index { get; }

    public IArgumentMissingInfo? Owner { get; }

    public bool IsNesting => true;

    public bool IsChoice => this.Owner != null;

    public string Usage { get; }

    public IReadOnlyList<string> HelpLines { get; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "It's the proposed way of handling missing cases for enum switches.")]
    public R<bool, GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings, bool useAliases)
    {
        var actualOption = this.options == null && this.IsRequired ? this.getDefault() : this.options;
        if (actualOption != null)
        {
            SerializationHelper.AppendNameOrAlias(stringBuilder, this.Name, this.Alias, useAliases);
            stringBuilder.Append(Constants.SpaceCharacter);
            var result = SerializeValue(actualOption, stringBuilder, settings, useAliases);

            if (result.IsSuccess)
            {
                return result.To(true);
            }

            var error = result.Error;
            return error.Type switch
            {
                GeneratorErrorType.SerializationException => throw new SerializationException(
                    this,
                    string.Format(settings.CultureInfo, Constants.NestedArgumentSerializationFormat, this.Usage, this.options, error.SerializationException),
                    error.SerializationException!),
                GeneratorErrorType.RequiredOptionMissing => result.To(false, innerGeneratorError => new GeneratorError(this, innerGeneratorError)),
                GeneratorErrorType.InnerGeneratorError => result.To(false, innerGeneratorError => new GeneratorError(this, innerGeneratorError)),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(error.Type),
                    error.Type,
                    string.Format(settings.CultureInfo, Constants.ErrorTypeNotSupportedFormat, error.Type)),
            };
        }

        return R.Success(false);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "It's the proposed way of handling missing cases for enum switches.")]
    public R<ParserError> DeserializeFrom(
        CommandLineArgumentsParser commandLineArgumentsParser,
        ArgumentList argumentList,
        ReadOnlySpan<char> value,
        Settings settings)
    {
        var options = this.getDefault();
        var result = this.DeserializeValue(commandLineArgumentsParser, argumentList, options, settings);
        if (result.IsSuccess)
        {
            this.setOptions(options);
            return result;
        }

        var error = result.Error;
        var argumentText = value.ToString();
        switch (error.Type)
        {
            case ParserErrorType.Info:
                return result.With(innerParserError => new ParserError(innerParserError, string.Format(settings.CultureInfo, Constants.InnerInfoErrorFormat, this.Usage, argumentText)));
            case ParserErrorType.InnerParserError:
                return result.With(innerParserError => new ParserError(innerParserError, string.Format(settings.CultureInfo, Constants.InnerParserErrorFormat, this.Usage, argumentText)));
            case ParserErrorType.RequiredArgumentMissing:
                return result.With(innerParserError => new ParserError(innerParserError, string.Format(settings.CultureInfo, Constants.InnerRequiredErrorFormat, this.Usage, argumentText)));
            case ParserErrorType.OptionArgumentMissing:
                return result.With(innerParserError => new ParserError(innerParserError, string.Format(settings.CultureInfo, Constants.InnerOptionArgumentErrorFormat, this.Usage, argumentText)));
            case ParserErrorType.UnknownOption:
                return result.With(innerParserError => new ParserError(innerParserError, string.Format(settings.CultureInfo, Constants.InnerInvalidOptionFormat, this.Usage, argumentText)));
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

    public void AppendHelpText(StringBuilder stringBuilder, Settings settings, int indent, TextSizes textSizes, bool isForVerb, bool isForNested)
    {
        HelpTextHelper.AppendHelpText(stringBuilder, settings, this, indent, textSizes.NameMaxLength, textSizes.AliasMaxLength, textSizes.HelpTextMaxLength, false, isForNested);
        var argumentsBuilder = new ArgumentsBuilder { Separators = settings.Separators };
        var defaultOptions = this.options ?? this.getDefault();
        defaultOptions.Configure(argumentsBuilder);
        CommandLineHelpGenerator.AppendCommandLineHelpText(argumentsBuilder, stringBuilder, textSizes, indent + 1, isForVerb, settings, true);
    }

    public void AppendMissingArgumentsHint(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine(this.Usage);
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

    private static R<GeneratorError> SerializeValue(TOptions options, StringBuilder stringBuilder, Settings settings, bool useAliases)
    {
        return CommandLineArgumentsGenerator.Generate(options, stringBuilder, settings, useAliases);
    }

    private R<ParserError> DeserializeValue(CommandLineArgumentsParser commandLineArgumentsParser, ArgumentList argumentList, TOptions options, Settings settings)
    {
        this.argumentsBuilder.PrepareBuilder(options, true);
        return commandLineArgumentsParser.Parse(this.argumentsBuilder, settings, argumentList, true);
    }
}