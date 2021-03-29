// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineArgumentsParser.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Sundew.Base.Primitives.Computation;
    using Sundew.Base.Text;

    internal class CommandLineArgumentsParser
    {
        internal const string HelpRequestedText = "Help requested";

        public Result.IfError<ParserError> Parse(
            ArgumentsBuilder argumentsBuilder,
            Settings settings,
            ArgumentList argumentList)
        {
            try
            {
                argumentsBuilder.CultureInfo = settings.CultureInfo;
                argumentsBuilder.Separators = settings.Separators;
                Result.IfError<ParserError>? currentResult = null;
                foreach (var argument in argumentList)
                {
                    if (argument[0] == Constants.ArgumentStartCharacter)
                    {
                        var actualArgument = argument;
                        var argumentValueSeparatorIndex = actualArgument.IndexOf(
                            Constants.EqualSignText,
                            StringComparison.OrdinalIgnoreCase);
                        if (argumentValueSeparatorIndex > -1)
                        {
                            argumentValueSeparatorIndex++;
                            actualArgument = argument.Substring(0, argumentValueSeparatorIndex);
                        }

                        if (argumentsBuilder.Options.TryGet(actualArgument, out var option))
                        {
                            argumentsBuilder.RequiredArguments.Remove(option);
                            if (option.Owner != null)
                            {
                                argumentsBuilder.RequiredArguments.Remove(option.Owner);
                            }

                            ReadOnlySpan<char> argumentValue = null;
                            if (argumentValueSeparatorIndex > -1)
                            {
                                argumentValue = RemoveValueEscapeIfNeeded(argument.Substring(argumentValueSeparatorIndex).AsSpan());
                            }
                            else if (argumentList.TryMoveNext(out var nextArgument))
                            {
                                argumentValue = RemoveValueEscapeIfNeeded(nextArgument.AsSpan());
                            }
                            else
                            {
                                return Result.Error(new ParserError(
                                    ParserErrorType.OptionArgumentMissing,
                                    string.Format(settings.CultureInfo, Constants.OptionArgumentMissingFormat, option.Usage)));
                            }

                            var deserializedResult = option.DeserializeFrom(this, argumentList, argumentValue, settings);
                            if (!deserializedResult)
                            {
                                return deserializedResult;
                            }

                            currentResult = deserializedResult;
                        }
                        else if (argumentsBuilder.Switches.TryGet(argument, out var @switch))
                        {
                            @switch.Set();
                        }
                        else if (currentResult != null)
                        {
                            argumentList.MoveBack();
                            if (argumentsBuilder.RequiredArguments.Any())
                            {
                                return CreateRequiredOptionMissingResult(argumentsBuilder);
                            }

                            return currentResult.Value;
                        }
                        else if (Constants.HelpRequestTexts.Contains(argument))
                        {
                            return Result.Error(new ParserError(ParserErrorType.HelpRequested, HelpRequestedText));
                        }
                        else
                        {
                            return Result.Error(new ParserError(
                                ParserErrorType.UnknownOption,
                                string.Format(settings.CultureInfo, Constants.UnknownOptionFormat, argument)));
                        }
                    }
                    else if (Constants.HelpRequestTexts.Contains(argument))
                    {
                        return Result.Error(new ParserError(ParserErrorType.HelpRequested, HelpRequestedText));
                    }
                    else if (argumentsBuilder.Values.HasValues)
                    {
                        argumentsBuilder.Values.DeserializeFrom(argumentsBuilder, argumentList, settings);
                    }
                    else
                    {
                        return Result.Error(new ParserError(ParserErrorType.UnknownVerb, argument));
                    }
                }
            }
            catch (SerializationException e)
            {
                return Result.Error(new ParserError(e));
            }

            if (argumentsBuilder.RequiredArguments.Any())
            {
                return CreateRequiredOptionMissingResult(argumentsBuilder);
            }

            return Result.Success();
        }

        internal static ReadOnlySpan<char> RemoveValueEscapeIfNeeded(ReadOnlySpan<char> span)
        {
            if (span.Length > 1 && span[0] == Constants.EscapeArgumentStartCharacter)
            {
                var secondCharacter = span[1];
                if (secondCharacter == Constants.ArgumentStartCharacter || secondCharacter == Constants.EscapeArgumentStartCharacter)
                {
                    return span.Slice(1);
                }
            }

            return span;
        }

        private static Result.IfError<ParserError> CreateRequiredOptionMissingResult(ArgumentsBuilder argumentsBuilder)
        {
            return Result.Error(
                new ParserError(
                    ParserErrorType.RequiredArgumentMissing,
                    GetMissingArguments(argumentsBuilder.RequiredArguments)));
        }

        private static string GetMissingArguments(List<IArgumentMissingInfo> requiredArguments)
        {
            if (requiredArguments.Count == 0)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            foreach (var requiredArgumentInfo in requiredArguments)
            {
                requiredArgumentInfo.AppendMissingArgumentsHint(stringBuilder);
            }

            return stringBuilder.ToString(Environment.NewLine);
        }
    }
}