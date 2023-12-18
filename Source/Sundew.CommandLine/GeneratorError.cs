// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorError.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

/// <summary>
/// Provides information when a generator error occurs.
/// </summary>
public sealed class GeneratorError
{
    internal GeneratorError(IArgumentInfo argumentInfo, GeneratorError innerGeneratorError)
    {
        this.Type = GeneratorErrorType.InnerGeneratorError;
        this.ArgumentInfo = argumentInfo;
        this.InnerGeneratorError = innerGeneratorError;
    }

    internal GeneratorError(IArgumentInfo argumentInfo, GeneratorErrorType generatorErrorType)
    {
        this.Type = generatorErrorType;
        this.ArgumentInfo = argumentInfo;
    }

    internal GeneratorError(GeneratorErrorType generatorErrorType)
    {
        this.Type = generatorErrorType;
    }

    internal GeneratorError(SerializationException serializationException)
    {
        this.Type = GeneratorErrorType.SerializationException;
        this.SerializationException = serializationException;
    }

    /// <summary>
    /// Gets the type.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    public GeneratorErrorType Type { get; }

    /// <summary>
    /// Gets the argument information.
    /// </summary>
    /// <value>
    /// The argument information.
    /// </value>
    public IArgumentInfo? ArgumentInfo { get; }

    /// <summary>
    /// Gets the inner generator error.
    /// </summary>
    /// <value>
    /// The inner generator error.
    /// </value>
    public GeneratorError? InnerGeneratorError { get; }

    /// <summary>
    /// Gets the serialization exception.
    /// </summary>
    /// <value>
    /// The serialization exception.
    /// </value>
    public SerializationException? SerializationException { get; }
}