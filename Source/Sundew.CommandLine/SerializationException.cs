// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationException.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System;

/// <summary>
/// An exception for indicating an error during serialization or deserialization of an argumentInfo.
/// </summary>
/// <seealso cref="System.Exception" />
public sealed class SerializationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SerializationException"/> class.
    /// </summary>
    /// <param name="argumentInfo">The argumentInfo.</param>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    internal SerializationException(IArgumentInfo? argumentInfo, string message, Exception innerException)
        : base(message, innerException)
    {
        this.ArgumentInfo = argumentInfo;
    }

    /// <summary>
    /// Gets the argumentInfo where the exception occured.
    /// </summary>
    /// <value>
    /// The argumentInfo.
    /// </value>
    public IArgumentInfo? ArgumentInfo { get; }
}