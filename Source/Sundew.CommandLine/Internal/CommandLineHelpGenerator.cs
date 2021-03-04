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
            AppendHelpText(stringBuilder, verbRegistries, argumentsAction, 0, 0, settings);
            return stringBuilder.ToString();
        }

        internal static void AppendHelpText<TSuccess, TError>(
            StringBuilder stringBuilder,
            VerbRegistry<TSuccess, TError>? verbRegistry,
            ArgumentsAction<TSuccess, TError>? argumentsAction,
            int indent,
            int verbMaxName,
            Settings settings)
        {
            if (verbRegistry != null)
            {
                if (verbRegistry.Verb != NullVerb.Instance)
                {
                    verbMaxName = Math.Max(verbMaxName, verbRegistry.Verb.Name.Length + (string.IsNullOrEmpty(verbRegistry.Verb.ShortName) ? 0 : verbRegistry.Verb.ShortName!.Length + 1));
                    stringBuilder.AppendFormat(
                        settings.CultureInfo,
                        $" {HelpTextHelper.GetIndentation(indent)}{{0,{-verbMaxName}}} | {verbRegistry.Verb.HelpText}",
                        $"{verbRegistry.Verb.Name}{(string.IsNullOrEmpty(verbRegistry.Verb.ShortName) ? string.Empty : $"/{verbRegistry.Verb.ShortName}")}");
                    stringBuilder.AppendLine();
                    verbRegistry.Builder.PrepareBuilder(verbRegistry.Verb, false);

                    AppendCommandLineHelpText(verbRegistry.Builder, stringBuilder, indent, 0, 0, true, settings, false);
                }

                if (verbRegistry.HasVerbs)
                {
                    if (indent == 0)
                    {
                        stringBuilder.AppendLine(Constants.VerbsText);
                    }

                    verbMaxName = GetVerbNameMax(verbRegistry, verbMaxName);
                    foreach (var registry in verbRegistry.HelpVerbs)
                    {
                        AppendHelpText(stringBuilder, registry, argumentsAction, indent + 1, verbMaxName, settings);
                    }
                }
            }

            if (argumentsAction != null)
            {
                var argumentsBuilder = argumentsAction.Builder;
                argumentsBuilder.PrepareBuilder(argumentsAction.Arguments, false);

                if (argumentsBuilder.Options.Any() || argumentsBuilder.Switches.Any() ||
                    argumentsBuilder.Values.HasValues)
                {
                    if (indent == 0)
                    {
                        stringBuilder.AppendLine(Constants.ArgumentsText);
                    }

                    AppendCommandLineHelpText(argumentsBuilder, stringBuilder, indent,  0, 0, false, settings, false);
                }
            }
        }

        internal static void AppendCommandLineHelpText(
            ArgumentsBuilder argumentsBuilder,
            StringBuilder stringBuilder,
            int indent,
            int nameMaxLength,
            int aliasMaxLength,
            bool isVerbArgument,
            Settings settings,
            bool isForNested)
        {
            var maxValues = argumentsBuilder.Values.HasValues
                ? argumentsBuilder.Values.Max(x => x.Name.Length) + Constants.LessThanText.Length +
                  Constants.GreaterThanText.Length
                : 0;
            if (argumentsBuilder.Options.Any() || argumentsBuilder.Switches.Any())
            {
                var argumentInfos = Concat(argumentsBuilder.Options, argumentsBuilder.Switches);
                nameMaxLength = Math.Max(argumentInfos.Max(x => (x.Name?.Length ?? 0) + (x.Separators.NameSeparator != Constants.SpaceCharacter ? 1 : 0) + (x.IsNesting ? 1 : 0) + (x.IsChoice ? 1 : 0)) + Constants.DashText.Length, nameMaxLength);
                aliasMaxLength = Math.Max(Math.Max(argumentInfos.Max(x => x.Alias.Length + (x.Separators.AliasSeparator != Constants.SpaceCharacter ? 1 : 0) + (x.IsNesting ? 1 : 0)) + Constants.DoubleDashText.Length, aliasMaxLength), maxValues - nameMaxLength - Constants.HelpSeparator.Length);
                maxValues = nameMaxLength + aliasMaxLength + Constants.HelpSeparator.Length;

                var maxHelpText = argumentsBuilder.Options.Any()
                    ? argumentsBuilder.Options.Max(x => x.HelpLines.Max(l => l.Length))
                    : 0;
                foreach (var option in GetOptions(argumentsBuilder.HelpOptions, argumentsBuilder.OptionsHelpOrder))
                {
                    option.AppendHelpText(stringBuilder, settings, indent, nameMaxLength, aliasMaxLength, maxHelpText, isVerbArgument, isForNested);
                }

                foreach (var @switch in argumentsBuilder.Switches.OrderBy(x => x.Index))
                {
                    @switch.AppendHelpText(stringBuilder, nameMaxLength, aliasMaxLength, indent, isVerbArgument, settings.CultureInfo);
                }
            }

            argumentsBuilder.Values.AppendHelpText(stringBuilder, maxValues, indent, isVerbArgument, settings);
        }

        private static IEnumerable<IArgumentHelpInfo> GetOptions(IReadOnlyList<IArgumentHelpInfo> options, OptionsHelpOrder optionsHelpOrder)
        {
            return optionsHelpOrder switch
            {
                OptionsHelpOrder.RequiredFirst => options.OrderByDescending(x => x.IsRequired).ThenBy(x => x.Index),
                OptionsHelpOrder.AsAdded => options.OrderBy(x => x.Index),
                _ => throw new ArgumentOutOfRangeException(nameof(optionsHelpOrder), optionsHelpOrder, $"{optionsHelpOrder} was out of range."),
            };
        }

        private static int GetVerbNameMax<TSuccess, TError>(VerbRegistry<TSuccess, TError> verbRegistry, int verbMaxName)
        {
            if (verbRegistry.HasVerbs)
            {
                return Math.Max(verbMaxName, verbRegistry.VerbRegistries.Max(x => x.Verb.Name.Length + (string.IsNullOrEmpty(verbRegistry.Verb.ShortName) ? 0 : verbRegistry.Verb.ShortName!.Length + 1)));
            }

            return verbMaxName;
        }

        private static IReadOnlyList<INamedArgumentInfo> Concat(
            IEnumerable<INamedArgumentInfo> options,
            IEnumerable<INamedArgumentInfo> switches)
        {
            return options.Concat(switches).ToArray();
        }
    }
}
