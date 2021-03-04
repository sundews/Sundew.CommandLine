// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpTextHelper.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Helpers
{
    using System;
    using System.Text;
    using Sundew.Base.Text;

    internal static class HelpTextHelper
    {
        public static void AppendHelpText(
            StringBuilder stringBuilder,
            Settings settings,
            IOption option,
            int indent,
            int nameMaxLength,
            int aliasMaxLength,
            int helpTextMaxLength,
            bool isForVerb,
            bool isForNested)
        {
            var indentText = GetIndentation(indent);
            var additionalIndentation = option.IsChoice ? 1 : 0;
            var namePadRight = -(nameMaxLength - additionalIndentation);
            var aliasPadRight = -aliasMaxLength;
            var helpTextPadRight = -helpTextMaxLength;
            stringBuilder.AppendFormat(
                settings.CultureInfo,
                $@"  {(isForVerb ? Constants.SpaceText : string.Empty)}{indentText}{' '.Repeat(additionalIndentation)}{{0,{namePadRight}}}{Constants.HelpSeparator}{{1,{aliasPadRight}}}{Constants.HelpSeparator}{{2,{helpTextPadRight}}}{Constants.HelpSeparator}",
                option.Name != null ? $"{Constants.ArgumentStartCharacter}{option.Name}{(option.Separators.NameSeparator != Constants.SpaceCharacter ? option.Separators.NameSeparator.ToString() : string.Empty)}" : string.Empty,
                $"{Constants.DoubleDashText}{option.Alias}{(option.Separators.AliasSeparator != Constants.SpaceCharacter ? option.Separators.AliasSeparator.ToString() : string.Empty)}",
                option.HelpLines[0]);
            option.AppendDefaultText(stringBuilder, settings, isForNested);
            stringBuilder.AppendLine();
            for (int i = 1; i < option.HelpLines.Count; i++)
            {
                stringBuilder.AppendFormat(
                    settings.CultureInfo,
                    $@"  {(isForVerb ? Constants.SpaceText : string.Empty)}{indentText}{' '.Repeat(additionalIndentation)}{{0,{namePadRight}}}{' '.Repeat(Constants.HelpSeparator.Length)}{{1,{aliasPadRight}}}{' '.Repeat(Constants.HelpSeparator.Length)}{{2,{helpTextPadRight}}}",
                    string.Empty,
                    string.Empty,
                    option.HelpLines[i]);
                stringBuilder.AppendLine();
            }
        }

        public static void AppendHelpText(StringBuilder stringBuilder, IValue value, int maxName, int indent, bool isForVerb, Settings settings)
        {
            var defaultOrRequiredText = GetDefaultOrRequiredText(value, settings);
            var maxNamePadRight = -maxName;
            stringBuilder.AppendFormat(
                settings.CultureInfo,
                $@"  {(isForVerb ? Constants.SpaceText : string.Empty)}{GetIndentation(indent)}{{0,{maxNamePadRight}}}{Constants.HelpSeparator}{value.HelpLines[0]}{Constants.HelpSeparator}{defaultOrRequiredText}",
                $"{Constants.LessThanText}{value.Name}{Constants.GreaterThanText}");
            stringBuilder.AppendLine();
            for (int i = 1; i < value.HelpLines.Count; i++)
            {
                stringBuilder.AppendFormat(
                    settings.CultureInfo,
                    $@"  {(isForVerb ? Constants.SpaceText : string.Empty)}{GetIndentation(indent)}{{0,{maxNamePadRight}}}{' '.Repeat(Constants.HelpSeparator.Length)}{value.HelpLines[i]}",
                    string.Empty);
                stringBuilder.AppendLine();
            }
        }

        public static string GetIndentation(int indent)
        {
            return Constants.SpaceCharacter.Repeat(indent * 2);
        }

        public static string GetUsage(string? name, string alias, Separators separators = default)
        {
            var stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(name))
            {
                stringBuilder.Append(Constants.ArgumentStartCharacter);
                stringBuilder.Append(name);
                if (separators.NameSeparator != Constants.SpaceCharacter)
                {
                    stringBuilder.Append(separators.NameSeparator);
                }
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(Constants.SlashCharacter);
            }

            if (!string.IsNullOrEmpty(alias))
            {
                stringBuilder.Append(Constants.DoubleDashText);
                stringBuilder.Append(alias);
                if (separators.AliasSeparator != Constants.SpaceCharacter)
                {
                    stringBuilder.Append(separators.AliasSeparator);
                }
            }

            return stringBuilder.ToString();
        }

        public static string GetDefaultOrRequiredText(IValue value, Settings settings)
        {
            if (value.IsRequired)
            {
                return Constants.RequiredText;
            }

            if (value.DefaultValueHelpText != null)
            {
                return Constants.DefaultText + value.DefaultValueHelpText;
            }

            var stringBuilder = new StringBuilder();
            value.SerializeTo(stringBuilder, settings);
            var defaultText = stringBuilder.ToString();
            if (string.IsNullOrWhiteSpace(defaultText))
            {
                return Constants.DefaultText + Constants.NoneText;
            }

            return Constants.DefaultText + defaultText;
        }

        public static string[] GetHelpLines(string helpText)
        {
            return helpText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}