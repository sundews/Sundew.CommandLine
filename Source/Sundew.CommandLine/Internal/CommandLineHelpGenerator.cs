// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineHelpGenerator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
                        $" {{0,{verbNamePadRight}}}{Constants.HelpSeparator}{verbRegistry.Verb.HelpText}",
                        $"{HelpTextHelper.GetIndentationText(indentation)}{verbRegistry.Verb.Name}{verbRegistry.Verb.ShortName.TransformIfNotNullOrEmpty(x => $"/{x}")}",
                        verbIndentation);
                    stringBuilder.AppendLine();

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
                        AppendHelpText(stringBuilder, registry, argumentsAction, textSizes, indent + 1, settings);
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
                        stringBuilder.AppendLine(Constants.ArgumentsText);
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
                foreach (var option in GetArgumentHelpInfos(argumentsBuilder.HelpOptions, argumentsBuilder.OptionsHelpOrder))
                {
                    option.AppendHelpText(stringBuilder, settings, indent, textSizes, isVerbArgument, isForNested);
                }
            }

            argumentsBuilder.Values.AppendHelpText(stringBuilder, textSizes, indent, isVerbArgument, settings);
        }

        private static TextSizes Measure<TSuccess, TError>(VerbRegistry<TSuccess, TError>? verbRegistry, ArgumentsAction<TSuccess, TError>? argumentsAction)
        {
            void GetArgumentsMax(ArgumentsBuilder argumentsBuilder, ref int nameMaxLength, ref int aliasMaxLength, ref int helpTextMaxLength, ref int valuesMaxLength, bool isForVerb, int indent)
            {
                var additionalIndent = isForVerb ? 1 : 0;

                var indentation = HelpTextHelper.GetIndentation(indent) + 1 + additionalIndent;
                var tempValuesMaxLength = Math.Max(
                    argumentsBuilder.Values.HasValues
                        ? argumentsBuilder.Values.Max(x => x.Name.Length) + Constants.LessThanText.Length + Constants.GreaterThanText.Length + indentation
                        : 0,
                    valuesMaxLength);

                var argumentInfos = Concat(argumentsBuilder.Options, argumentsBuilder.Switches);
                nameMaxLength = Math.Max(argumentInfos.Any() ? argumentInfos.Max(x => (x.Name?.Length ?? 0) + (x.Separators.NameSeparator != Constants.SpaceCharacter ? 1 : 0) + (x.IsNesting ? 1 : 0) + (x.IsChoice ? 1 : 0)) + Constants.DashText.Length + indentation : 0, nameMaxLength);
                aliasMaxLength = Math.Max(Math.Max(argumentInfos.Any() ? argumentInfos.Max(x => x.Alias.Length + (x.Separators.AliasSeparator != Constants.SpaceCharacter ? 1 : 0) + (x.IsNesting ? 1 : 0)) + Constants.DoubleDashText.Length : 0, aliasMaxLength), tempValuesMaxLength - nameMaxLength - Constants.HelpSeparator.Length);
                valuesMaxLength = nameMaxLength + Constants.HelpSeparator.Length + aliasMaxLength;

                var helpTextItems = argumentInfos.Select(x => x.HelpLines).Concat(argumentsBuilder.Values.Select(x => x.HelpLines)).ToArray();
                helpTextMaxLength = Math.Max(
                    helpTextItems.Any()
                    ? helpTextItems.Max(x => x.Max(l => l.Length))
                    : 0,
                    helpTextMaxLength);
            }

            void GetMax(VerbRegistry<TSuccess, TError>? verbRegistry, ArgumentsAction<TSuccess, TError>? argumentsAction, ref int verbMaxName, ref int maxName, ref int maxAlias, ref int helpTextMaxLength, ref int valuesMaxLength, int indent)
            {
                if (verbRegistry != null)
                {
                    if (verbRegistry.Verb != NullVerb.Instance)
                    {
                        verbMaxName = Math.Max(verbMaxName, verbRegistry.Verb.Name.Length + verbRegistry.Verb.ShortName.GetAdjustedLengthOrDefault(1, 0) + HelpTextHelper.GetIndentation(indent));
                        verbRegistry.Builder.PrepareBuilder(verbRegistry.Verb, false);
                        GetArgumentsMax(verbRegistry.Builder, ref maxName, ref maxAlias, ref helpTextMaxLength, ref valuesMaxLength, true, indent);
                    }

                    if (verbRegistry.HasVerbs)
                    {
                        foreach (var registry in verbRegistry.HelpVerbs)
                        {
                            GetMax(registry, argumentsAction, ref verbMaxName, ref maxName, ref maxAlias, ref helpTextMaxLength, ref valuesMaxLength, indent + 1);
                        }
                    }
                }

                if (argumentsAction != null)
                {
                    argumentsAction.Builder.PrepareBuilder(argumentsAction.Arguments, false);
                    GetArgumentsMax(argumentsAction.Builder, ref maxName, ref maxAlias, ref helpTextMaxLength, ref valuesMaxLength, false, indent);
                }
            }

            var verbNameMaxLength = 0;
            var nameMaxLength = 0;
            var aliasMaxLength = 0;
            var helpTextMaxLength = 0;
            var valuesMaxLength = 0;
            GetMax(verbRegistry, argumentsAction, ref verbNameMaxLength, ref nameMaxLength, ref aliasMaxLength, ref helpTextMaxLength, ref valuesMaxLength, 0);

            return new TextSizes(verbNameMaxLength, nameMaxLength, aliasMaxLength, helpTextMaxLength, valuesMaxLength);
        }

        private static IEnumerable<IArgumentHelpInfo> GetArgumentHelpInfos(
            IReadOnlyList<IArgumentHelpInfo> options,
            OptionsHelpOrder optionsHelpOrder)
        {
            return optionsHelpOrder switch
            {
                OptionsHelpOrder.RequiredFirst => options.OrderByDescending(x => x.IsRequired).ThenBy(x => x.Index),
                OptionsHelpOrder.AsAdded => options.OrderBy(x => x.Index),
                _ => throw new ArgumentOutOfRangeException(nameof(optionsHelpOrder), optionsHelpOrder, $"{optionsHelpOrder} was out of range."),
            };
        }

        private static IReadOnlyList<INamedArgumentInfo> Concat(
            IEnumerable<INamedArgumentInfo> options,
            IEnumerable<INamedArgumentInfo> switches)
        {
            return options.Concat(switches).ToArray();
        }
    }
}
