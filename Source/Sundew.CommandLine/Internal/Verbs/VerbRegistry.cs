// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerbRegistry.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Verbs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Sundew.Base.Primitives.Computation;
    using Sundew.Base.Text;
    using Sundew.CommandLine.Internal.Helpers;

    internal class VerbRegistry<TSuccess, TError> : IVerbBuilder<TSuccess, TError>, IVerbRegistry<TSuccess, TError>
    {
        private readonly Dictionary<ReadOnlyMemory<char>, VerbRegistry<TSuccess, TError>> verbRegistries = new(ReadOnlyMemoryCharEqualityComparer.Instance);
        private readonly List<VerbRegistry<TSuccess, TError>> helpVerbses = new();

        public VerbRegistry(IVerb verb, Func<IVerb, ValueTask<Result<TSuccess, ParserError<TError>>>> handler, Action<IVerbBuilder<TSuccess, TError>>? verbBuilderAction)
        {
            this.Verb = verb;
            this.Handler = handler;
            this.HelpLines = HelpTextHelper.GetHelpLines(verb.HelpText);
            verbBuilderAction?.Invoke(this);
        }

        public ArgumentsBuilder Builder { get; } = new();

        public bool HasVerbs => this.verbRegistries.Any();

        public IEnumerable<VerbRegistry<TSuccess, TError>> VerbRegistries => this.verbRegistries.Values;

        public IEnumerable<VerbRegistry<TSuccess, TError>> HelpVerbs => this.helpVerbses;

        public IVerb Verb { get; }

        public Func<IVerb, ValueTask<Result<TSuccess, ParserError<TError>>>> Handler { get; }

        public string[] HelpLines { get; }

        public TVerb AddVerb<TVerb>(
            TVerb verb,
            Func<TVerb, Result<TSuccess, ParserError<TError>>> verbHandler)
            where TVerb : IVerb
        {
            this.AddVerb(verb, verbHandler, null);
            return verb;
        }

        public TVerb AddVerb<TVerb>(
            TVerb verb,
            Func<TVerb, ValueTask<Result<TSuccess, ParserError<TError>>>> verbHandler)
            where TVerb : IVerb
        {
            this.AddVerb(verb, verbHandler, null);
            return verb;
        }

        public TVerb AddVerb<TVerb>(
            TVerb verb,
            Func<TVerb, Result<TSuccess, ParserError<TError>>> verbHandler,
            Action<IVerbBuilder<TSuccess, TError>>? verbBuilderAction)
            where TVerb : IVerb
        {
            this.AddVerb(
                verb,
                parsedVerb => new ValueTask<Result<TSuccess, ParserError<TError>>>(verbHandler(parsedVerb)),
                verbBuilderAction);
            return verb;
        }

        public TVerb AddVerb<TVerb>(
            TVerb verb,
            Func<TVerb, ValueTask<Result<TSuccess, ParserError<TError>>>> verbHandler,
            Action<IVerbBuilder<TSuccess, TError>>? verbBuilderAction)
            where TVerb : IVerb
        {
            var verbRegistry = new VerbRegistry<TSuccess, TError>(verb, parsedVerb => verbHandler((TVerb)parsedVerb), verbBuilderAction);
            this.verbRegistries.Add(verb.Name.AsMemory(), verbRegistry);
            this.helpVerbses.Add(verbRegistry);
            if (!verb.ShortName.IsNullOrEmpty())
            {
                this.verbRegistries.Add(verb.ShortName!.AsMemory(), verbRegistry);
            }

            return verb;
        }

        public bool TryGetValue(ReadOnlyMemory<char> verb, out VerbRegistry<TSuccess, TError> verbRegistry)
        {
            return this.verbRegistries.TryGetValue(verb, out verbRegistry);
        }
    }
}