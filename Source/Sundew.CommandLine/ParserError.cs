// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserError.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    using System;
    using System.Text;
    using Sundew.CommandLine.Internal;

    /// <summary>
    /// Provides information when a parser error occurs.
    /// </summary>
    public class ParserError
    {
        private const char SpaceCharacter = ' ';
        private const string UnknownErrorText = "Unknown error";

        internal ParserError(ParserErrorType parserErrorType, string message)
        {
            this.Type = parserErrorType;
            this.Message = message;
        }

        internal ParserError(SerializationException serializationException)
        {
            this.Type = ParserErrorType.SerializationException;
            this.SerializationException = serializationException;
            this.Message = Constants.SerializationExceptionErrorText;
        }

        internal ParserError(ParserError innerParserError, string message)
        {
            this.Type = ParserErrorType.InnerParserError;
            this.InnerParserError = innerParserError;
            this.Message = message;
        }

        internal ParserError(ParserErrorType type, SerializationException? serializationException, string message, ParserError? innerParserError)
        {
            this.Type = type;
            this.SerializationException = serializationException;
            this.Message = message;
            this.InnerParserError = innerParserError;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public ParserErrorType Type { get; }

        /// <summary>
        /// Gets the serialization exception.
        /// </summary>
        /// <value>
        /// The serialization exception.
        /// </value>
        public SerializationException? SerializationException { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; }

        /// <summary>
        /// Gets the inner parser error.
        /// </summary>
        /// <value>
        /// The inner parser error.
        /// </value>
        public ParserError? InnerParserError { get; }

        /// <summary>
        /// Create a parser error from the provided error information.
        /// </summary>
        /// <typeparam name="TInfo">The type of the error information.</typeparam>
        /// <param name="info">The error information.</param>
        /// <returns>A new <see cref="ParserError{TInfo}"/>.</returns>
        public static ParserError<TInfo> From<TInfo>(TInfo info)
        {
            return new ParserError<TInfo>(info);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            AppendParserErrorText(stringBuilder, this, 2);
            return stringBuilder.ToString(0, stringBuilder.Length - Environment.NewLine.Length);
        }

        internal static bool AppendParserErrorText(StringBuilder stringBuilder, ParserError parserError, int indent)
        {
            switch (parserError.Type)
            {
                case ParserErrorType.UnknownVerb:
                    stringBuilder.Append(SpaceCharacter, indent);
                    stringBuilder.AppendLine($"The verb {parserError.Message} is unknown.");
                    break;
                case ParserErrorType.UnknownOption:
                    stringBuilder.Append(SpaceCharacter, indent);
                    stringBuilder.AppendLine(parserError.Message);
                    break;
                case ParserErrorType.RequiredArgumentMissing:
                    stringBuilder.Append(SpaceCharacter, indent);
                    stringBuilder.AppendLine($"The required options were missing: {parserError.Message}.");
                    break;
                case ParserErrorType.SerializationException:
                    stringBuilder.Append(SpaceCharacter, indent);
                    stringBuilder.AppendLine(parserError.Message);
                    stringBuilder.Append(SpaceCharacter, indent);
                    stringBuilder.AppendLine(parserError.SerializationException!.ToString());
                    break;
                case ParserErrorType.HelpRequested:
                    stringBuilder.AppendLine(parserError.Message);
                    break;
                case ParserErrorType.Info:
                    return false;
                case ParserErrorType.OptionArgumentMissing:
                    stringBuilder.Append(SpaceCharacter, indent);
                    stringBuilder.AppendLine(parserError.Message);
                    break;
                case ParserErrorType.InnerParserError:
                    if (parserError.InnerParserError != null)
                    {
                        stringBuilder.Append(SpaceCharacter, indent);
                        stringBuilder.AppendLine(parserError.Message);
                        AppendParserErrorText(stringBuilder, parserError.InnerParserError, indent + 2);
                    }

                    break;
                case ParserErrorType.InvalidStartIndex:
                    stringBuilder.Append(SpaceCharacter, indent);
                    stringBuilder.AppendLine(parserError.Message);
                    break;
                case ParserErrorType.ArgumentsAndVerbsAreNotConfigured:
                    stringBuilder.Append(SpaceCharacter, indent);
                    stringBuilder.AppendLine(parserError.Message);
                    break;
                case ParserErrorType.ArgumentsNotConfiguredOrUnknownVerb:
                    stringBuilder.Append(SpaceCharacter, indent);
                    stringBuilder.AppendLine(parserError.Message);
                    break;
                default:
                    stringBuilder.Append(SpaceCharacter, indent);
                    stringBuilder.AppendLine(UnknownErrorText);
                    break;
            }

            return true;
        }
    }
}