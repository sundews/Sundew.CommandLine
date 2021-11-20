// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentsBuilderException.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    using System;

    /// <summary>Exception thrown when the <see cref="IArgumentsBuilder"/> detects an invalid pattern.</summary>
    /// <seealso cref="System.Exception" />
    public class ArgumentsBuilderException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ArgumentsBuilderException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ArgumentsBuilderException(string message)
        : base(message)
        {
        }
    }
}