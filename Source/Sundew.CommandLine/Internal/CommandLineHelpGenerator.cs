// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineHelpGenerator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sundew.Base.Collections;
using Sundew.Base.Text;
using Sundew.CommandLine.Internal.Extensions;
using Sundew.CommandLine.Internal.Helpers;
using Sundew.CommandLine.Internal.Verbs;

internal static class CommandLineHelpGenerator
{
    public static string CreateHelpText<TSuccess, TError>(
        VerbRegistry<TSuccess, TError> verbRegistries,
        ArgumentsAction<TSuccess, TError>? argumentsAction,
        Settings settings)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(Constants.HelpText);
        var textSizes = Measure(verbRegistries, argumentsAction);
        AppendHelpText(stringBuilder, verbRegistries, argumentsAction, textSizes, 0, settings);
        return stringBuilder.ToString();
    }

    internal static void AppendHelpText<TSuccess, TError>(
        StringBuilder stringBuilder,
        VerbRegistry<TSuccess, TError>? verbRegistry,
        ArgumentsAction<TSuccess, TError>? argumentsAction,
        TextSizes textSizes,
        int indent,
        Settings settings)
    {
        if (verbRegistry != null)
        {
            if (verbRegistry.Verb != NullVerb.Instance)
            {
                var verbIndentation = Constants.SpaceCharacter.Repeat(indent);
                var indentation = HelpTextHelper.GetIndentation(indent);
                var verbNamePadRight = -Math.Max(textSizes.VerbNameMaxLength, textSizes.ValuesMaxLength);
                stringBuilder.AppendFormat(
                    settings.CultureInfo,
                    $" {{0,{verbNamePadRight}}} {verbRegistry.HelpLines[0]}",
                    $"{HelpTextHelper.GetIndentationText(indentation)}{verbRegistry.Verb.Name}{verbRegistry.Verb.ShortName.TransformIfNotNullOrEmpty(x => $"/{x}")}",
                    verbIndentation);
                stringBuilder.AppendLine();
                for (int i = 1; i < verbRegistry.HelpLines.Length; i++)
                {
                    stringBuilder.AppendFormat(
                        settings.CultureInfo,
                        $@" {{0,{verbNamePadRight}}} {{1}}",
                        Constants.SpaceText,
                        verbRegistry.HelpLines[i]);
                    stringBuilder.AppendLine();
                }

                AppendCommandLineHelpText(verbRegistry.Builder, stringBuilder, textSizes, indent, true, settings, false);
            }

            if (verbRegistry.HasVerbs)
            {
                if (indent == 0)
                {
                    stringBuilder.AppendLine(Constants.VerbsText);
                }

                foreach (var registry in verbRegistry.HelpVerbs)
                {
                    AppendHelpText(stringBuilder, registry, null, textSizes, indent + 1, settings);
                }
            }
        }

        if (argumentsAction != null)
        {
            var argumentsBuilder = argumentsAction.Builder;
            if (argumentsBuilder.Options.Any() || argumentsBuilder.Switches.Any() || argumentsBuilder.Values.HasValues)
            {
                if (indent == 0)
                {
                    var argumentsPadRight = -(textSizes.ValuesMaxLength + 1);
                    stringBuilder.AppendFormat($"{{0,{argumentsPadRight}}} {argumentsAction.HelpLines[0]}", Constants.ArgumentsText);
                    stringBuilder.AppendLine();
                    for (int i = 1; i < argumentsAction.HelpLines.Length; i++)
                    {
                        stringBuilder.AppendFormat(
                            settings.CultureInfo,
                            $@" {{0,{argumentsPadRight}}} {{1}}",
                            Constants.SpaceText,
                            argumentsAction.HelpLines[i]);
                        stringBuilder.AppendLine();
                    }
                }

                AppendCommandLineHelpText(argumentsBuilder, stringBuilder, textSizes, indent, false, settings, false);
            }
        }
    }

    internal static void AppendCommandLineHelpText(
        ArgumentsBuilder argumentsBuilder,
        StringBuilder stringBuilder,
        TextSizes textSizes,
        int indent,
        bool isVerbArgument,
        Settings settings,
        bool isForNested)
    {
        if (argumentsBuilder.Options.Any() || argumentsBuilder.Switches.Any())
        {
            foreach (var option in argumentsBuilder.HelpOptions.OrderBy(x => x.Index))
            {
                option.AppendHelpText(stringBuilder, settings, indent, textSizes, isVerbArgument, isForNested);
            }
        }

        argumentsBuilder.Values.AppendHelpText(stringBuilder, textSizes, indent, isVerbArgument, settings);
    }

    private static TextSizes Measure<TSuccess, TError>(VerbRegistry<TSuccess, TError>? verbRegistry, ArgumentsAction<TSuccess, TError>? argumentsAction)
    {
        void GetArgumentsMax(ArgumentsBuilder argumentsBuilder, ref TextSizes textSizes, bool isForVerb, int indent)
        {
            var additionalIndent = isForVerb ? 1 : 0;

            var indentation = HelpTextHelper.GetIndentation(indent) + 1 + additionalIndent;
            var tempValuesMaxLength = Math.Max(
                argumentsBuilder.Values.HasValues
                    ? argumentsBuilder.Values.Max(x => x.Name.Length) + Constants.LessThanText.Length + Constants.GreaterThanText.Length + indentation
                    : 0,
                textSizes.ValuesMaxLength);

            var argumentInfos = Concat(argumentsBuilder.Options, argumentsBuilder.Switches);
            textSizes.NameMaxLength = Math.Max(argumentInfos.Any() ? argumentInfos.Max(x => (x.Name?.Length ?? 0) + (x.Separators.NameSeparator != Constants.SpaceCharacter ? 1 : 0) + (x.IsNesting ? 1 : 0) + (x.IsChoice ? 1 : 0)) + Constants.DashText.Length + indentation : 0, textSizes.NameMaxLength);
            textSizes.AliasMaxLength = Math.Max(Math.Max(argumentInfos.Any() ? argumentInfos.Max(x => x.Alias.Length + (x.Separators.AliasSeparator != Constants.SpaceCharacter ? 1 : 0) + (x.IsNesting ? 1 : 0)) + Constants.DoubleDashText.Length : 0, textSizes.AliasMaxLength), tempValuesMaxLength - textSizes.NameMaxLength - Constants.HelpSeparator.Length);
            textSizes.ValuesMaxLength = textSizes.NameMaxLength + Constants.HelpSeparator.Length + textSizes.AliasMaxLength;

            var helpTextItems = argumentInfos.SelectMany(x => x.HelpLines.Select(x => x.Length)).Concat(argumentsBuilder.Values.SelectMany(x => x.HelpLines.Select(x => x.Length))).ToArray();
            textSizes.HelpTextMaxLength = Math.Max(
                helpTextItems.Any()
                    ? helpTextItems.Max()
                    : 0,
                textSizes.HelpTextMaxLength);
        }

        void GetMax(VerbRegistry<TSuccess, TError>? verbRegistry, ArgumentsAction<TSuccess, TError>? argumentsAction, ref TextSizes textSizes, int indent)
        {
            if (verbRegistry != null)
            {
                if (verbRegistry.Verb != NullVerb.Instance)
                {
                    textSizes.VerbNameMaxLength = Math.Max(textSizes.VerbNameMaxLength, verbRegistry.Verb.Name.Length + verbRegistry.Verb.ShortName.GetAdjustedLengthOrDefault(1, 0) + HelpTextHelper.GetIndentation(indent));
                    verbRegistry.Builder.PrepareBuilder(verbRegistry.Verb, false);
                    GetArgumentsMax(verbRegistry.Builder, ref textSizes, true, indent);
                }

                if (verbRegistry.HasVerbs)
                {
                    foreach (var registry in verbRegistry.HelpVerbs)
                    {
                        GetMax(registry, argumentsAction, ref textSizes, indent + 1);
                    }
                }
            }

            if (argumentsAction != null)
            {
                argumentsAction.Builder.PrepareBuilder(argumentsAction.Arguments, false);
                GetArgumentsMax(argumentsAction.Builder, ref textSizes, false, indent);
            }
        }

        var textSizes = new TextSizes(0, 0, 0, 0, 0);
        GetMax(verbRegistry, argumentsAction, ref textSizes, 0);
        return textSizes;
    }

    private static IReadOnlyList<INamedArgumentInfo> Concat(
        IEnumerable<INamedArgumentInfo> options,
        IEnumerable<INamedArgumentInfo> switches)
    {
        return options.Concat(switches).ToArray();
    }
}