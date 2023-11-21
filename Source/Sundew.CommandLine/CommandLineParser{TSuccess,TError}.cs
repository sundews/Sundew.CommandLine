// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineParser{TSuccess,TError}.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sundew.Base.Collections;
using Sundew.Base.Primitives.Computation;
using Sundew.CommandLine.Extensions;
using Sundew.CommandLine.Internal;
using Sundew.CommandLine.Internal.Verbs;

/// <summary>
/// Implementation of the command line parser.
/// </summary>
/// <typeparam name="TSuccess">The type of the success.</typeparam>
/// <typeparam name="TError">The type of the error.</typeparam>
/// <seealso cref="Sundew.CommandLine.ICommandLineParser{TSuccess, TError}" />
/// <seealso cref="Sundew.CommandLine.ICommandLineBuilder{TSuccess, TError}" />
public sealed class CommandLineParser<TSuccess, TError> : ICommandLineParser<TSuccess, TError>, ICommandLineBuilder<TSuccess, TError>, ICommandLineHelper
{
    private const string ArgumentsOrVerbsWereNotConfiguredText = "Arguments and/or verbs were not configured.";
    private const string ArgumentsWereNotConfiguredOrAnUnknownVerbWasUsedText = "Either arguments were not configured or an unknown verb was used.";
    private readonly CommandLineArgumentsParser commandLineArgumentsParser = new();
    private readonly VerbRegistry<TSuccess, TError> verbRegistry = new(NullVerb.Instance, verb => R.From(false, default(TSuccess)!, new ParserError<TError>(ParserErrorType.UnknownVerb, "Null verb")), null);
    private ArgumentsAction<TSuccess, TError>? argumentsAction;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineParser{TSuccess, TError}"/> class.
    /// </summary>
    public CommandLineParser()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineParser{TSuccess, TError}"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public CommandLineParser(Settings settings)
        : this()
    {
        this.Settings = settings;
    }

    /// <summary>
    /// Gets the settings.
    /// </summary>
    public Settings Settings { get; } = new Settings();

    /// <summary>
    /// Adds the verb.
    /// </summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <returns>The verb.</returns>
    public TVerb AddVerb<TVerb>(TVerb verb, Func<TVerb, R<TSuccess, ParserError<TError>>> verbHandler)
        where TVerb : IVerb
    {
        return this.AddVerb(verb, verbHandler, null);
    }

    /// <summary>Adds the verb.</summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <param name="verbBuilderAction">The verb builder function.</param>
    /// <returns>The verb.</returns>
    public TVerb AddVerb<TVerb>(TVerb verb, Func<TVerb, R<TSuccess, ParserError<TError>>> verbHandler, Action<IVerbBuilder<TSuccess, TError>>? verbBuilderAction)
        where TVerb : IVerb
    {
        this.verbRegistry.AddVerb(verb, verbHandler, verbBuilderAction);
        return verb;
    }

    /// <summary>
    /// Adds the verb.
    /// </summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <returns>The verb.</returns>
    public TVerb AddVerb<TVerb>(TVerb verb, Func<TVerb, ValueTask<R<TSuccess, ParserError<TError>>>> verbHandler)
        where TVerb : IVerb
    {
        return this.AddVerb(verb, verbHandler, null);
    }

    /// <summary>Adds the verb.</summary>
    /// <typeparam name="TVerb">The type of the verb.</typeparam>
    /// <param name="verb">The verb.</param>
    /// <param name="verbHandler">The verb handler.</param>
    /// <param name="verbBuilderAction">The verb builder function.</param>
    /// <returns>The verb.</returns>
    public TVerb AddVerb<TVerb>(TVerb verb, Func<TVerb, ValueTask<R<TSuccess, ParserError<TError>>>> verbHandler, Action<IVerbBuilder<TSuccess, TError>>? verbBuilderAction)
        where TVerb : IVerb
    {
        this.verbRegistry.AddVerb(verb, verbHandler, verbBuilderAction);
        return verb;
    }

    /// <summary>
    /// Specifies the default arguments for parsing (non verb mode).
    /// </summary>
    /// <typeparam name="TArguments">The type of the arguments.</typeparam>
    /// <param name="arguments">The arguments.</param>
    /// <param name="argumentsHandler">The arguments handler.</param>
    /// <returns>The arguments.</returns>
    public TArguments WithArguments<TArguments>(TArguments arguments, Func<TArguments, R<TSuccess, ParserError<TError>>> argumentsHandler)
        where TArguments : IArguments
    {
        this.argumentsAction = new ArgumentsAction<TSuccess, TError>(arguments, x => argumentsHandler((TArguments)x));
        return arguments;
    }

    /// <summary>
    /// Specifies the default arguments for parsing (non verb mode).
    /// </summary>
    /// <typeparam name="TArguments">The type of the arguments.</typeparam>
    /// <param name="arguments">The arguments.</param>
    /// <param name="argumentsHandler">The arguments handler.</param>
    /// <returns>The arguments.</returns>
    public TArguments WithArguments<TArguments>(TArguments arguments, Func<TArguments, ValueTask<R<TSuccess, ParserError<TError>>>> argumentsHandler)
        where TArguments : IArguments
    {
        this.argumentsAction = new ArgumentsAction<TSuccess, TError>(arguments, x => argumentsHandler((TArguments)x));
        return arguments;
    }

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    public ValueTask<R<TSuccess, ParserError<TError>>> ParseAsync(IReadOnlyList<string> arguments)
    {
        return this.ParseAsync(arguments.ToArray(x => x.AsMemory()), 0);
    }

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    public ValueTask<R<TSuccess, ParserError<TError>>> ParseAsync(IReadOnlyList<ReadOnlyMemory<char>> arguments)
    {
        return this.ParseAsync(arguments, 0);
    }

    /// <summary>Parses the specified arguments.</summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    public ValueTask<R<TSuccess, ParserError<TError>>> ParseAsync(string? arguments)
    {
        return this.ParseAsync(arguments, 0);
    }

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="startIndex">The argument index at which to start parsing.</param>
    /// <returns>The parser result.</returns>
    public ValueTask<R<TSuccess, ParserError<TError>>> ParseAsync(string? arguments, int startIndex)
    {
        var argumentArray = arguments.AsMemory().ParseCommandLineArguments().ToArray();
        return this.ParseAsync(argumentArray, startIndex);
    }

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="startIndex">The start index.</param>
    /// <returns>The parser result.</returns>
    public async ValueTask<R<TSuccess, ParserError<TError>>> ParseAsync(IReadOnlyList<ReadOnlyMemory<char>> arguments, int startIndex)
    {
        if (startIndex < 0)
        {
            return R.Error(new ParserError<TError>(ParserErrorType.InvalidStartIndex, $"StartIndex must be greater than 0 (Actual: {startIndex})."));
        }

        var argumentList = new ArgumentList(arguments, startIndex);
        if (this.verbRegistry.HasVerbs)
        {
            if (argumentList.TryPeek(out var argument))
            {
                VerbRegistry<TSuccess, TError>? currentVerbAction = null;
                IVerbRegistry<TSuccess, TError> currentVerbRegistry = this.verbRegistry;
                while (currentVerbRegistry.TryGetValue(argument, out var verbInfo))
                {
                    currentVerbRegistry = currentVerbAction = verbInfo;
                    if (!argumentList.TryMoveNext(out argument))
                    {
                        break;
                    }
                }

                if (currentVerbAction != null)
                {
                    var verbResult = await this.ParseArguments(argumentList, currentVerbAction, currentVerbAction.Verb, currentVerbAction.Handler).ConfigureAwait(false);
                    return CheckResultForHelpRequestedError(verbResult, currentVerbAction, this.argumentsAction, this.Settings);
                }
            }
        }

        R<TSuccess, ParserError<TError>> result;
        if (this.argumentsAction == null)
        {
            if (this.verbRegistry.HasVerbs)
            {
                if (Constants.HelpRequestTexts.Contains(argumentList.FirstOrDefault(), ReadOnlyMemoryCharEqualityComparer.Instance))
                {
                    result = R.Error(new ParserError<TError>(ParserErrorType.HelpRequested, CommandLineArgumentsParser.HelpRequestedText));
                }
                else
                {
                    result = R.Error(new ParserError<TError>(
                        ParserErrorType.ArgumentsNotConfiguredOrUnknownVerb,
                        ArgumentsWereNotConfiguredOrAnUnknownVerbWasUsedText));
                }
            }
            else
            {
                result = R.Error(new ParserError<TError>(
                    ParserErrorType.ArgumentsAndVerbsAreNotConfigured, ArgumentsOrVerbsWereNotConfiguredText));
            }
        }
        else
        {
            result = await this.ParseArguments(argumentList, this.argumentsAction, this.argumentsAction.Arguments, this.argumentsAction.Handler).ConfigureAwait(false);
        }

        return CheckResultForHelpRequestedError(result, this.verbRegistry, this.argumentsAction, this.Settings);
    }

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    public R<TSuccess, ParserError<TError>> Parse(IReadOnlyList<string> arguments)
    {
        return this.Parse(arguments.ToArray(x => x.AsMemory()), 0);
    }

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    public R<TSuccess, ParserError<TError>> Parse(IReadOnlyList<ReadOnlyMemory<char>> arguments)
    {
        return this.Parse(arguments, 0);
    }

    /// <summary>Parses the specified arguments.</summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The parser result.</returns>
    public R<TSuccess, ParserError<TError>> Parse(string? arguments)
    {
        return this.Parse(arguments, 0);
    }

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="startIndex">The argument index at which to start parsing.</param>
    /// <returns>The parser result.</returns>
    public R<TSuccess, ParserError<TError>> Parse(string? arguments, int startIndex)
    {
        var argumentArray = arguments.AsMemory().ParseCommandLineArguments().ToArray();
        return this.Parse(argumentArray, startIndex);
    }

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="startIndex">The start index.</param>
    /// <returns>The parser result.</returns>
    public R<TSuccess, ParserError<TError>> Parse(IReadOnlyList<string> arguments, int startIndex)
    {
        return this.Parse(arguments.ToArray(x => x.AsMemory()), startIndex);
    }

    /// <summary>
    /// Parses the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="startIndex">The start index.</param>
    /// <returns>The parser result.</returns>
    public R<TSuccess, ParserError<TError>> Parse(IReadOnlyList<ReadOnlyMemory<char>> arguments, int startIndex)
    {
        var parseTask = this.ParseAsync(arguments, startIndex).AsTask();
        parseTask.Wait();
        return parseTask.Result;
    }

    /// <summary>
    /// Creates the help text.
    /// </summary>
    /// <returns>The help text.</returns>
    public string CreateHelpText()
    {
        return CommandLineHelpGenerator.CreateHelpText(this.verbRegistry, this.argumentsAction, this.Settings);
    }

    private static R<TSuccess, ParserError<TError>> CheckResultForHelpRequestedError(
        R<TSuccess, ParserError<TError>> result,
        VerbRegistry<TSuccess, TError> verbRegistry,
        ArgumentsAction<TSuccess, TError>? argumentsAction,
        Settings settings)
    {
        if (!result.IsSuccess)
        {
            if (result.Error.Type == ParserErrorType.HelpRequested)
            {
                return R.Error(new ParserError<TError>(ParserErrorType.HelpRequested, CommandLineHelpGenerator.CreateHelpText(verbRegistry, argumentsAction, settings)));
            }
        }

        return result;
    }

    private ValueTask<R<TSuccess, ParserError<TError>>> ParseArguments<TArguments>(
        ArgumentList argumentList,
        IArgumentsBuilderProvider argumentsBuilderProvider,
        TArguments argumentsDefinition,
        Func<TArguments, ValueTask<R<TSuccess, ParserError<TError>>>> argumentsHandler)
        where TArguments : IArguments
    {
        var argumentsBuilder = argumentsBuilderProvider.Builder;
        argumentsBuilder.PrepareBuilder(argumentsDefinition, true);

        var result = this.commandLineArgumentsParser.Parse(argumentsBuilder, this.Settings, argumentList, false);
        if (!result)
        {
            return result.To(default(TSuccess)!, parserError => new ParserError<TError>(parserError));
        }

        return argumentsHandler(argumentsDefinition);
    }
}