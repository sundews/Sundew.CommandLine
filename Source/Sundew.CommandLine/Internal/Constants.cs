// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System;

    internal static class Constants
    {
        public const string ArgumentSerializationErrorFormat = "Serialization error: Option: {0}.";
        public const string ArgumentDeserializationErrorFormat = "Deserialization error: Option: {0} with the argument: {1}.";
        public const string ListSerializationErrorFormat = "Serialization error: Item {0} at index {1}.";
        public const string ListDeserializationErrorFormat = "Deserialization error: Item {0}";
        public const string ErrorTypeNotSupportedFormat = "The generator error type is not supported: {0}";
        public const string SpaceText = " ";
        public const char SpaceCharacter = ' ';
        public const char EscapeArgumentStartCharacter = '\\';
        public const char SlashCharacter = '/';
        public const char ArgumentStartCharacter = '-';
        public const char NullCharacter = '\0';
        public const string DashText = "-";
        public const string DoubleDashText = "--";
        public const string EqualSignText = "=";
        public const string DoubleQuotes = @"""";
        public const string HelpText = "Help";
        public const string DefaultText = "Default: ";
        public const string ArgumentsText = " Arguments:";
        public const string NoneText = "[none]";
        public const string VerbsText = " Verbs:";
        public const string RequiredText = "Required";
        public const string OptionArgumentMissingFormat = "The argument for the option: {0} is missing.";
        public const string UnknownOptionFormat = "The option does not exist: {0}";
        public const string InnerInvalidOptionFormat = "Inner option does not exist while parsing: {0} with argument: {1}.";
        public const string InnerParserErrorFormat = "Inner parser error occured while parsing: {0} with argument: {1}.";
        public const string InnerInfoErrorFormat = "Inner info error occured while parsing: {0} with argument: {1}.";
        public const string InnerOptionArgumentErrorFormat = "Inner option argument was missing while parsing: {0} with argument: {1}.";
        public const string InnerRequiredErrorFormat = "Inner required argument was missing while parsing: {0} with argument: {1}.";
        public const string SerializationExceptionErrorText = "Serialization exception occured:";
        public const string OnlyASingleValueIsAllowedErrorText = "Only a single value is allowed.";
        public const string ValueSerializationErrorText = "The value could not be serialized, see inner exception.";
        public const string SeeBelowText = "see below";
        public const string HelpSeparator = " | ";
        public const string LessThanText = "<";
        public const string GreaterThanText = ">";
        public static readonly string NestedArgumentSerializationFormat = $"Serialization error: Option: {{0}} with value: {{1}}.{Environment.NewLine}Inner exception:{Environment.NewLine}{{2}}";
        public static readonly string NestedArgumentDeserializationFormat = $"Deserialization error: Option: {{0}} with argument: {{1}}.{Environment.NewLine}Inner exception:{Environment.NewLine}{{2}}";
        public static readonly string[] HelpRequestTexts = { "?", "-?", "-h", "--help" };
    }
}
