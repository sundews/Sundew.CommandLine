// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpTextHelper.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Helpers
{
    using System.Globalization;
    using System.Text;
    using Sundew.Base.Text;

    internal static class HelpTextHelper
    {
        public static void AppendHelpText(
            StringBuilder stringBuilder,
            Settings settings,
            IOption option,
            int maxName,
            int maxAlias,
            int maxHelpText,
            int indent,
            bool isForVerb)
        {
            var indentText = GetIndentation(indent);
            var maxNamePadRight = -maxName;
            var maxAliasPadRight = -maxAlias;
            var maxHelpTextPadRight = -maxHelpText;
            stringBuilder.AppendFormat(
                settings.CultureInfo,
                $@"  {(isForVerb ? Constants.SpaceText : string.Empty)}{indentText}{{0,{maxNamePadRight}}}{Constants.HelpSeparator}{{1,{maxAliasPadRight}}}{Constants.HelpSeparator}{{2,{maxHelpTextPadRight}}}{Constants.HelpSeparator}",
                option.Name != null ? $"{Constants.ArgumentStartCharacter}{option.Name}{(option.Separators.NameSeparator != Constants.SpaceCharacter ? option.Separators.NameSeparator.ToString() : string.Empty)}" : string.Empty,
                option.Alias != null ? $"{Constants.DoubleDashText}{option.Alias}{(option.Separators.AliasSeparator != Constants.SpaceCharacter ? option.Separators.AliasSeparator.ToString() : string.Empty)}" : string.Empty,
                option.HelpText);
            option.AppendDefaultText(stringBuilder, settings, indent > 0);
        }

        public static void AppendHelpText(StringBuilder stringBuilder, IValue value, int maxName, int indent, bool isForVerb, Settings settings)
        {
            var defaultOrRequiredText = GetDefaultOrRequiredText(value, settings);
            var maxNamePadRight = -maxName;
            stringBuilder.AppendFormat(
                settings.CultureInfo,
                $@"  {(isForVerb ? Constants.SpaceText : string.Empty)}{GetIndentation(indent)}{{0,{maxNamePadRight}}}{Constants.HelpSeparator}{value.HelpText}{Constants.HelpSeparator}{defaultOrRequiredText}",
                $"{Constants.LessThanText}{value.Name}{Constants.GreaterThanText}");
            stringBuilder.AppendLine();
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

            var stringBuilder = new StringBuilder();
            value.SerializeTo(stringBuilder, settings);
            var defaultText = stringBuilder.ToString();
            if (string.IsNullOrWhiteSpace(defaultText))
            {
                return Constants.DefaultText + Constants.NoneText;
            }

            return Constants.DefaultText + defaultText;
        }
    }
}