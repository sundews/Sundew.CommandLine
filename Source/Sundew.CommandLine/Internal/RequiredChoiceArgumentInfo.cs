// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequiredChoiceArgumentInfo.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Sundew.CommandLine.Internal.Helpers;

    internal class RequiredChoiceArgumentInfo : IArgumentHelpInfo, IArgumentMissingInfo
    {
        private const string Or = " or ";
        private readonly string name;
        private readonly List<IArgumentInfo> choiceOptions;

        public RequiredChoiceArgumentInfo(string name, List<IArgumentInfo> choiceOptions, int index)
        {
            this.name = name;
            this.choiceOptions = choiceOptions;
            this.Index = index;
        }

        public bool IsRequired => true;

        public int Index { get; }

        public void AppendHelpText(StringBuilder stringBuilder, Settings settings, int indent, TextSizes textSizes, bool isForVerb, bool isForNested)
        {
            var indentText = HelpTextHelper.GetIndentationText(HelpTextHelper.GetIndentation(indent));
            var namePadRight = -(textSizes.NameMaxLength + textSizes.AliasMaxLength + textSizes.HelpTextMaxLength + 5);
            stringBuilder.AppendFormat(
                settings.CultureInfo,
                $@"  {(isForVerb ? Constants.SpaceText : string.Empty)}{indentText}{{0,{namePadRight}}}{Constants.HelpSeparator}{Constants.RequiredText}",
                this.name);
            stringBuilder.AppendLine();
        }

        public void AppendMissingArgumentsHint(StringBuilder stringBuilder)
        {
            stringBuilder.Append(this.choiceOptions[0].Usage);
            foreach (var choiceOption in this.choiceOptions.Skip(1))
            {
                stringBuilder.Append(Or);
                stringBuilder.Append(choiceOption.Usage);
            }

            stringBuilder.AppendLine();
        }
    }
}