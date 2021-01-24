// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserErrorType.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    /// <summary>
    /// Specifies the type of a parser error.
    /// </summary>
    public enum ParserErrorType
    {
        /// <summary>
        /// The verb specified by the command line was not registered.
        /// The verb name is specified by the message.
        /// </summary>
        UnknownVerb = 1,

        /// <summary>The arguments and verbs are not configured.</summary>
        ArgumentsAndVerbsAreNotConfigured,

        /// <summary>The arguments not configured or unknown verb.</summary>
        ArgumentsNotConfiguredOrUnknownVerb = ArgumentsAndVerbsAreNotConfigured | UnknownVerb,

        /// <summary>
        /// The required option was missing.
        /// The missing options are specified by the message.
        /// </summary>
        RequiredArgumentMissing,

        /// <summary>
        /// The option argument is missing.
        /// See the message for details.
        /// </summary>
        OptionArgumentMissing,

        /// <summary>
        /// A serialization exception occured.
        /// </summary>
        SerializationException,

        /// <summary>
        /// Help was requested.
        /// The help text is provide by the message.
        /// </summary>
        HelpRequested,

        /// <summary>
        /// Refer to the info property.
        /// See the info property.
        /// </summary>
        Info,

        /// <summary>
        /// The inner parser error.
        /// </summary>
        InnerParserError,

        /// <summary>The only single value allowed.</summary>
        OnlySingleValueAllowed,

        /// <summary>
        /// The specified option is invalid.
        /// </summary>
        UnknownOption,

        /// <summary>The invalid start index.</summary>
        InvalidStartIndex,
    }
}