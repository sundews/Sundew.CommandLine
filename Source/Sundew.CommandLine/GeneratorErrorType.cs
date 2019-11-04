// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorErrorType.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    /// <summary>
    /// Specifies the type of generator error.
    /// </summary>
    public enum GeneratorErrorType
    {
        /// <summary>
        /// A serialization exception occured.
        /// </summary>
        SerializationException,

        /// <summary>
        /// The required option missing.
        /// </summary>
        RequiredOptionMissing,

        /// <summary>The required values missing.</summary>
        RequiredValuesMissing,

        /// <summary>
        /// The inner generator error.
        /// </summary>
        InnerGeneratorError,

        /// <summary>
        /// The verbs missing.
        /// </summary>
        VerbsMissing,
    }
}