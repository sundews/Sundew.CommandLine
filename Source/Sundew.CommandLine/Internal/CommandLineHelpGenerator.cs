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
                    verbMaxName = Math.Max(verbMaxName, verbRegistry.Verb.Name.Length);
                    stringBuilder.AppendFormat(
                        settings.CultureInfo,
                        $" {HelpTextHelper.GetIndentation(indent)}{{0,{-verbMaxName}}} | {verbRegistry.Verb.HelpText}",
                        verbRegistry.Verb.Name);
                    stringBuilder.AppendLine();
                    verbRegistry.Builder.PrepareBuilder(verbRegistry.Verb, false);

                    AppendCommandLineHelpText(verbRegistry.Builder, stringBuilder, indent, 0, 0, true, settings);
                }

                if (verbRegistry.HasVerbs)
                {
                    if (indent == 0)
                    {
                        stringBuilder.AppendLine(Constants.VerbsText);
                    }

                    verbMaxName = GetVerbNameMax(verbRegistry, verbMaxName);
                    foreach (var registry in verbRegistry.VerbRegistries)
                    {
                        AppendHelpText(stringBuilder, registry, argumentsAction, indent + 1, verbMaxName, settings);
                    }
                }
            }

            if (argumentsAction != null)
            {
                var argumentsBuilder = argumentsAction.Builder;
                argumentsBuilder.PrepareBuilder(argumentsAction.Arguments, false);

                if (argumentsBuilder.Options.Any() || argumentsBuilder.Switches.Any() || argumentsBuilder.Values.HasValues)
                {
                    if (indent == 0)
                    {
                        stringBuilder.AppendLine(Constants.ArgumentsText);
                    }

                    AppendCommandLineHelpText(argumentsBuilder, stringBuilder, indent, 0, 0, false, settings);
                }
            }
        }

        internal static void AppendCommandLineHelpText(
            ArgumentsBuilder argumentsBuilder,
            StringBuilder stringBuilder,
            int indent,
            int maxName,
            int maxAlias,
            bool isVerbArgument,
            Settings settings)
        {
            var maxValues = argumentsBuilder.Values.HasValues ? argumentsBuilder.Values.Max(x => x.Name.Length) + Constants.LessThanText.Length + Constants.GreaterThanText.Length : 0;
            if (argumentsBuilder.Options.Any() || argumentsBuilder.Switches.Any())
            {
                var argumentInfos = Concat(argumentsBuilder.Options, argumentsBuilder.Switches);
                maxName = Math.Max(argumentInfos.Max(x => (x.Name?.Length ?? 0) + (x.Separators.NameSeparator != Constants.SpaceCharacter ? 1 : 0) + (x.IsNesting ? 1 : 0)) + Constants.DashText.Length, maxName);
                maxAlias = Math.Max(Math.Max(argumentInfos.Max(x => x.Alias.Length + (x.Separators.AliasSeparator != Constants.SpaceCharacter ? 1 : 0) + (x.IsNesting ? 1 : 0)) + Constants.DoubleDashText.Length, maxAlias), maxValues - maxName - Constants.HelpSeparator.Length);
                maxValues = maxName + maxAlias + Constants.HelpSeparator.Length;

                var maxHelpText = argumentsBuilder.Options.Any() ? argumentsBuilder.Options.Max(x => x.HelpLines.Max(l => l.Length)) : 0;
                foreach (var option in argumentsBuilder.Options.OrderByDescending(x => x.IsRequired))
                {
                    option.AppendHelpText(stringBuilder, settings, maxName, maxAlias, maxHelpText, indent, isVerbArgument);
                }

                foreach (var @switch in argumentsBuilder.Switches.OrderBy(x => x.Alias))
                {
                    @switch.AppendHelpText(stringBuilder, maxName, maxAlias, indent, isVerbArgument, settings.CultureInfo);
                }
            }

            argumentsBuilder.Values.AppendHelpText(stringBuilder, maxValues, indent, isVerbArgument, settings);
        }

        private static int GetVerbNameMax<TSuccess, TError>(VerbRegistry<TSuccess, TError> verbRegistry, int verbMaxName)
        {
            if (verbRegistry.HasVerbs)
            {
                return Math.Max(verbMaxName, verbRegistry.VerbRegistries.Max(x => x.Verb.Name.Length));
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
