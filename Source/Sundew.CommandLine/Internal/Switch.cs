// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Switch.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System;
    using System.Globalization;
    using System.Text;
    using Sundew.CommandLine.Internal.Helpers;

    internal sealed class Switch : INamedArgumentInfo
    {
        private readonly Action<bool> setValue;

        public Switch(string? name, string alias, bool isSet, Action<bool> setValue, string helpText)
        {
            this.Name = name;
            this.Alias = alias;
            this.DefaultValue = this.IsSet = isSet;
            this.setValue = setValue;
            this.HelpText = helpText;
            this.Usage = HelpTextHelper.GetUsage(name, alias);
        }

        public string? Name { get; }

        public string Alias { get; }

        public Separators Separators => default;

        public string Usage { get; }

        public string HelpText { get; }

        public bool IsNesting => false;

        public bool IsSet { get; private set; }

        public bool DefaultValue { get; }

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

        public void AppendHelpText(StringBuilder stringBuilder, int maxName, int maxAlias, int indent, bool isForVerb, CultureInfo cultureInfo)
        {
            var maxNamePadRight = -maxName;
            var maxAliasPadRight = -maxAlias;
            stringBuilder.AppendFormat(
                cultureInfo,
                $@"  {(isForVerb ? Constants.SpaceText : string.Empty)}{HelpTextHelper.GetIndentation(indent)}{{0,{maxNamePadRight}}}{Constants.HelpSeparator}{{1,{maxAliasPadRight}}}{Constants.HelpSeparator}{{2}}{Environment.NewLine}",
                this.Name != null ? $"{Constants.ArgumentStartCharacter}{this.Name}" : string.Empty,
                this.Alias != null ? $"{Constants.DoubleDashText}{this.Alias}" : string.Empty,
                this.HelpText);
        }
    }
}