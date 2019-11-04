// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Separators.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    using System;
    using System.Text.RegularExpressions;
    using Sundew.CommandLine.Internal;

    /// <summary>
    /// Defines the argument-value separators.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "By design, not intended for equality.")]
    public readonly struct Separators
    {
        private static readonly Regex InvalidSeparatorRegex = new Regex("\"|-");
        private readonly char nameSeparator;
        private readonly char aliasSeparator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Separators"/> struct.
        /// </summary>
        /// <param name="nameSeparator">The name separators.</param>
        /// <param name="aliasSeparator">The alias separators.</param>
        public Separators(char nameSeparator, char aliasSeparator)
        {
            this.nameSeparator = nameSeparator;
            this.aliasSeparator = aliasSeparator;
            if (InvalidSeparatorRegex.IsMatch(nameSeparator.ToString()))
            {
                throw new NotSupportedException($"The character: {nameSeparator} is not supported as a argument-value separators.");
            }

            if (InvalidSeparatorRegex.IsMatch(aliasSeparator.ToString()))
            {
                throw new NotSupportedException($"The character: {aliasSeparator} is not supported as a argument-value separators.");
            }
        }

        /// <summary>
        /// Gets the name separators.
        /// </summary>
        public char NameSeparator
        {
            get
            {
                if (this.nameSeparator == Constants.NullCharacter)
                {
                    return Constants.SpaceCharacter;
                }

                return this.nameSeparator;
            }
        }

        /// <summary>
        /// Gets the alias separators.
        /// </summary>
        public char AliasSeparator
        {
            get
            {
                if (this.aliasSeparator == Constants.NullCharacter)
                {
                    return Constants.SpaceCharacter;
                }

                return this.aliasSeparator;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the separators is the default value separators.
        /// </summary>
        public bool IsDefault => Equals(this, default(Separators));

        /// <summary>
        /// Creates a <see cref="Separators"/> with name and alias set to the specified separators.
        /// </summary>
        /// <param name="nameSeparator">The name separators.</param>
        /// <param name="aliasSeparator">The alias separators.</param>
        /// <returns>A new <see cref="Separators"/>.</returns>
        public static Separators Create(char nameSeparator = Constants.SpaceCharacter, char aliasSeparator = Constants.SpaceCharacter)
        {
            return new Separators(nameSeparator, aliasSeparator);
        }

        /// <summary>
        /// Creates a <see cref="Separators"/> with name and alias set to the specified separators.
        /// </summary>
        /// <param name="separator">The separators.</param>
        /// <returns>A new <see cref="Separators"/>.</returns>
        public static Separators ForNameAndAlias(char separator)
        {
            return new Separators(separator, separator);
        }

        /// <summary>
        /// Creates a <see cref="Separators"/> with name set to the specified separators and the alias separators set to space.
        /// </summary>
        /// <param name="nameSeparator">The name separators.</param>
        /// <returns>A new <see cref="Separators"/>.</returns>
        public static Separators ForName(char nameSeparator)
        {
            return new Separators(nameSeparator, Constants.SpaceCharacter);
        }

        /// <summary>
        /// Creates a <see cref="Separators"/> with alias set to the specified separators and the name separators set to space.
        /// </summary>
        /// <param name="aliasSeparator">The alias separators.</param>
        /// <returns>A new <see cref="Separators"/>.</returns>
        public static Separators ForAlias(char aliasSeparator)
        {
            return new Separators(Constants.SpaceCharacter, aliasSeparator);
        }
    }
}