// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationHelper.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Sundew.CommandLine.Internal.Extensions;

    internal static class SerializationHelper
    {
        public static void DeserializeTo<TItem>(IList<TItem> list, Deserialize<TItem> deserialize, ReadOnlySpan<char> argument, Settings settings)
        {
            try
            {
                list.Add(deserialize(argument, settings.CultureInfo));
            }
            catch (Exception e)
            {
                throw new SerializationException(null, string.Format(settings.CultureInfo, Constants.ListDeserializationErrorFormat, argument.ToString()), e);
            }
        }

        public static bool SerializeTo<TItem>(IListSerializationInfo<TItem> listSerializationInfo, IList<TItem> list, StringBuilder stringBuilder, Settings settings)
        {
            if (list.Count > 0)
            {
                var value = SerializeValue(listSerializationInfo.Serialize, list, 0, settings);
                if (!value.IsEmpty)
                {
                    AppendValue(stringBuilder, value, listSerializationInfo.UseDoubleQuotes);
                }
            }

            for (var index = 1; index < list.Count; index++)
            {
                var item = list[index];
                var value = listSerializationInfo.Serialize(item, settings.CultureInfo);
                if (value.IsEmpty)
                {
                    continue;
                }

                stringBuilder.Append(Constants.SpaceCharacter);

                AppendValue(stringBuilder, value, listSerializationInfo.UseDoubleQuotes);
            }

            return list.Any();
        }

        public static bool AppendNameOrAlias(StringBuilder stringBuilder, string name, string alias, bool preferAliases)
        {
            if (string.IsNullOrEmpty(name) || (preferAliases && !string.IsNullOrEmpty(alias)))
            {
                stringBuilder.Append(Constants.DoubleDashText);
                stringBuilder.Append(alias);
                return true;
            }

            stringBuilder.Append(Constants.ArgumentStartCharacter);
            stringBuilder.Append(name);
            return false;
        }

        internal static void AppendQuotes(StringBuilder stringBuilder, bool useDoubleQuotes)
        {
            if (useDoubleQuotes)
            {
                stringBuilder.Append(Constants.DoubleQuotesCharacter);
            }
        }

        internal static void EscapeValuesIfNeeded(StringBuilder stringBuilder, ReadOnlySpan<char> value)
        {
            if (!value.IsEmpty && (value[0] == Constants.EscapeArgumentStartCharacter || value[0] == Constants.ArgumentStartCharacter))
            {
                stringBuilder.Append(Constants.EscapeArgumentStartCharacter);
            }
        }

        private static ReadOnlySpan<char> SerializeValue<TItem>(Serialize<TItem> serialize, IList<TItem> list, int index, Settings settings)
        {
            var item = list[index];
            try
            {
                return serialize(item, settings.CultureInfo);
            }
            catch (Exception e)
            {
                throw new SerializationException(null, string.Format(settings.CultureInfo, Constants.ListSerializationErrorFormat, item, index), e);
            }
        }

        private static void AppendValue(StringBuilder stringBuilder, ReadOnlySpan<char> value, bool useDoubleQuotes)
        {
            AppendQuotes(stringBuilder, useDoubleQuotes);
            EscapeValuesIfNeeded(stringBuilder, value);
            stringBuilder.Append(value);
            AppendQuotes(stringBuilder, useDoubleQuotes);
        }
    }
}