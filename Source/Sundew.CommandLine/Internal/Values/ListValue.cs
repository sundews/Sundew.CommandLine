// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListValue.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Values
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Sundew.Base.Computation;
    using Sundew.Base.Text;
    using Sundew.CommandLine.Internal.Helpers;

    internal sealed class ListValue<TValue> : IValue, IListSerializationInfo<TValue>
    {
        private readonly Deserialize<TValue> deserialize;

        public ListValue(
            string name,
            IList<TValue> list,
            Serialize<TValue> serialize,
            Deserialize<TValue> deserialize,
            bool isRequired,
            string helpText,
            bool useDoubleQuotes)
        {
            this.Name = name.Uncapitalize(CultureInfo.InvariantCulture);
            this.List = list;
            this.deserialize = deserialize;
            this.Serialize = serialize;
            this.IsRequired = isRequired;
            this.HelpText = helpText;
            this.UseDoubleQuotes = useDoubleQuotes;
        }

        public string Name { get; }

        public IList<TValue> List { get; }

        public Serialize<TValue> Serialize { get; }

        public bool IsRequired { get; }

        public bool IsList => true;

        public bool UseDoubleQuotes { get; }

        public string Usage => $"<{this.Name}>";

        public string HelpText { get; }

        public bool IsNesting { get; } = false;

        public Result.IfError<GeneratorError> SerializeTo(StringBuilder stringBuilder, Settings settings)
        {
            var wasSerialized = SerializationHelper.SerializeTo(this, stringBuilder, settings);
            if (!wasSerialized && this.IsRequired)
            {
                return Result.Error(new GeneratorError(GeneratorErrorType.RequiredValuesMissing));
            }

            return Result.Success();
        }

        public Result.IfError<ParserError> DeserializeFrom(ReadOnlySpan<char> argument, Settings settings)
        {
            SerializationHelper.DeserializeTo(this.List, this.deserialize, argument, settings);
            return Result.Success();
        }
    }
}