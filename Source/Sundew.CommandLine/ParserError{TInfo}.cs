// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserError{TInfo}.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System;
using System.Text;
using Sundew.CommandLine.Internal;

/// <summary>
/// Provides information when a parser error occurs.
/// </summary>
/// <typeparam name="TInfo">The type of the error information.</typeparam>
/// <seealso cref="Sundew.CommandLine.ParserError" />
public sealed class ParserError<TInfo> : ParserError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParserError{TInfo}"/> class.
    /// </summary>
    /// <param name="info">The error information.</param>
    public ParserError(TInfo info)
        : base(ParserErrorType.Info, $"Info: {info}")
    {
        this.Info = info;
    }

    internal ParserError(ParserError innerParserError)
        : base(innerParserError.Type, innerParserError.SerializationException, innerParserError.Message, innerParserError.InnerParserError)
    {
    }

    internal ParserError(ParserErrorType type, string message)
        : base(type, message)
    {
    }

    internal ParserError(SerializationException serializationException)
        : base(serializationException)
    {
    }

    /// <summary>
    /// Gets the error information.
    /// </summary>
    /// <value>
    /// The error information.
    /// </value>
    public TInfo Info { get; } = default!;

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return this.ToString(null);
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <param name="infoFunc">The information function.</param>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    public string ToString(Func<TInfo, int, string>? infoFunc)
    {
        var stringBuilder = new StringBuilder(this.Type != ParserErrorType.HelpRequested ? "Error:" + Environment.NewLine : string.Empty);
        var indent = 2;
        if (!AppendParserErrorText(stringBuilder, this, indent))
        {
            if (infoFunc == null)
            {
                stringBuilder.Append(Constants.SpaceCharacter, indent);
                stringBuilder.AppendLine($"Error info: {this.Info}");
            }
            else
            {
                stringBuilder.AppendLine(infoFunc(this.Info, indent));
            }
        }

        return stringBuilder.ToString(0, stringBuilder.Length - Environment.NewLine.Length);
    }
}