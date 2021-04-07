// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineGenerator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    using System.Text;
    using Sundew.Base.Primitives.Computation;
    using Sundew.CommandLine.Internal;

    /// <summary>
    /// A command line generator to generate command lines for the command line parser.
    /// </summary>
    /// <seealso cref="Sundew.CommandLine.ICommandLineGenerator" />
    public sealed class CommandLineGenerator : ICommandLineGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineGenerator"/> class.
        /// </summary>
        public CommandLineGenerator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineGenerator"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public CommandLineGenerator(Settings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        public Settings Settings { get; } = new();

        /// <summary>Generates the specified verb.</summary>
        /// <param name="verb">The verb.</param>
        /// <returns>The generator result.</returns>
        public Result<string, GeneratorError> Generate(IVerb verb)
        {
            return this.Generate(verb, false);
        }

        /// <summary>Generates the specified verb.</summary>
        /// <param name="verb">The verb.</param>
        /// <param name="useAliases">if set to <c>true</c> [use aliases].</param>
        /// <returns>The generator result.</returns>
        public Result<string, GeneratorError> Generate(IVerb? verb, bool useAliases)
        {
            if (verb == null)
            {
                return Result.Error(new GeneratorError(GeneratorErrorType.VerbsMissing));
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(verb.Name);
            stringBuilder.Append(Constants.SpaceCharacter);
            while (verb.NextVerb != null)
            {
                verb = verb.NextVerb;
                stringBuilder.Append(verb.Name);
                stringBuilder.Append(Constants.SpaceCharacter);
            }

            return CommandLineArgumentsGenerator.Generate(verb, stringBuilder, this.Settings, useAliases)
                .WithValue(stringBuilder.ToString());
        }

        /// <summary>Generates the specified verb.</summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The generator result.</returns>
        public Result<string, GeneratorError> Generate(IArguments arguments)
        {
            return this.Generate(arguments, false);
        }

        /// <summary>Generates the specified verb.</summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="useAliases">if set to <c>true</c> [use aliases].</param>
        /// <returns>The generator result.</returns>
        public Result<string, GeneratorError> Generate(IArguments arguments, bool useAliases)
        {
            var stringBuilder = new StringBuilder();
            return CommandLineArgumentsGenerator.Generate(arguments, stringBuilder, this.Settings, useAliases)
                .WithValue(stringBuilder.ToString());
        }
    }
}