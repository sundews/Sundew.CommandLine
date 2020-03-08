// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineArgumentsGenerator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System.Text;
    using Sundew.Base.Computation;

    internal class CommandLineArgumentsGenerator
    {
        public static Result.IfError<GeneratorError> Generate(IArguments arguments, StringBuilder stringBuilder, Settings settings, bool useAliases)
        {
            var argumentsBuilder = new ArgumentsBuilder
            {
                Separators = settings.Separators,
                CultureInfo = settings.CultureInfo,
            };

            arguments.Configure(argumentsBuilder);
            try
            {
                foreach (var option in argumentsBuilder.Options)
                {
                    var serializeResult = option.SerializeTo(stringBuilder, settings, useAliases);
                    if (!serializeResult)
                    {
                        return serializeResult;
                    }

                    stringBuilder.Append(Constants.SpaceText);
                }

                foreach (var @switch in argumentsBuilder.Switches)
                {
                    if (@switch.IsSet)
                    {
                        @switch.SerializeTo(stringBuilder, useAliases);
                        stringBuilder.Append(Constants.SpaceText);
                    }
                }

                if (argumentsBuilder.Values.HasValues)
                {
                    var valuesSerializeResult = argumentsBuilder.Values.SerializeTo(stringBuilder, settings);
                    if (!valuesSerializeResult)
                    {
                        return valuesSerializeResult;
                    }
                }

                var lastCharacterIndex = stringBuilder.Length - 1;
                if (stringBuilder.Length > 0 && stringBuilder[lastCharacterIndex] == Constants.SpaceCharacter)
                {
                    stringBuilder.Remove(lastCharacterIndex, 1);
                }
            }
            catch (SerializationException e)
            {
                return Result.Error(new GeneratorError(e));
            }

            return Result.Success();
        }
    }
}