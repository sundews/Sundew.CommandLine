// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Switch.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Sundew.CommandLine.Internal.Helpers;

    internal sealed class Switch : INamedArgumentInfo, IArgumentHelpInfo
    {
        private readonly Action<bool> setValue;

        public Switch(string? name, string alias, bool isSet, Action<bool> setValue, string helpText, int index, IArgumentMissingInfo? owner)
        {
            this.Name = name;
            this.Alias = alias;
            this.DefaultValue = this.IsSet = isSet;
            this.setValue = setValue;
            this.Index = index;
            this.Owner = owner;
            this.HelpLines = HelpTextHelper.GetHelpLines(helpText);
            this.Usage = HelpTextHelper.GetUsage(name, alias);
        }

        public string? Name { get; }

        public string Alias { get; }

        public Separators Separators => default;

        public string Usage { get; }

        public IReadOnlyList<string> HelpLines { get; }

        public bool IsNesting => false;

        public bool IsChoice => this.Owner != null;

        public bool IsSet { get; private set; }

        public bool DefaultValue { get; }

        public bool IsRequired => false;

        public int Index { get; }

        public IArgumentMissingInfo? Owner { get; }

        public void ResetToDefault(CultureInfo cultureInfo)
        {
            this.IsSet = this.DefaultValue;
            this.setValue(this.DefaultValue);
        }

        public void Set()
        {
            try
            {
                this.IsSet = true;
                this.setValue(true);
            }
            catch (Exception e)
            {
                throw new SerializationException(this, Constants.ArgumentDeserializationErrorFormat, e);
            }
        }

        public void SerializeTo(StringBuilder stringBuilder, bool useAliases)
        {
            SerializationHelper.AppendNameOrAlias(stringBuilder, this.Name, this.Alias, useAliases);
        }

        public void AppendHelpText(StringBuilder stringBuilder, Settings settings, int indent, TextSizes textSizes, bool isForVerb, bool isForNested)
        {
            var cultureInfo = settings.CultureInfo;
            var additionalIndent = isForVerb ? 1 : 0;
            additionalIndent += this.IsChoice ? 1 : 0;
            var maxNamePadRight = -textSizes.NameMaxLength;
            var maxAliasPadRight = -textSizes.AliasMaxLength;
            var indentationText = HelpTextHelper.GetIndentationText(HelpTextHelper.GetIndentation(indent) + additionalIndent + 1);
            stringBuilder.AppendFormat(
                cultureInfo,
                $@" {{0,{maxNamePadRight}}}{Constants.HelpSeparator}{{1,{maxAliasPadRight}}}{Constants.HelpSeparator}{{2}}",
                this.Name != null ? $"{indentationText}{Constants.ArgumentStartCharacter}{this.Name}" : string.Empty,
                this.Alias != null ? $"{Constants.DoubleDashText}{this.Alias}" : string.Empty,
                this.HelpLines[0]);
            for (int i = 1; i < this.HelpLines.Count; i++)
            {
                stringBuilder.AppendFormat(
                    cultureInfo,
                    $@"{Environment.NewLine} {{0,{maxNamePadRight}}}   {{1,{maxAliasPadRight}}}   {{2}}",
                    indentationText,
                    string.Empty,
                    this.HelpLines[i]);
            }

            stringBuilder.AppendLine();
        }
    }
}