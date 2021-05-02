// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentsAction.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal
{
    using System;
    using System.Threading.Tasks;
    using Sundew.Base.Primitives.Computation;
    using Sundew.CommandLine.Internal.Helpers;

    internal class ArgumentsAction<TSuccess, TError> : IArgumentsBuilderProvider
    {
        public ArgumentsAction(IArguments arguments, Func<IArguments, Result<TSuccess, ParserError<TError>>> handler)
         : this(arguments, arguments => new ValueTask<Result<TSuccess, ParserError<TError>>>(handler(arguments)))
        {
        }

        public ArgumentsAction(IArguments arguments, Func<IArguments, ValueTask<Result<TSuccess, ParserError<TError>>>> handler)
        {
            this.Arguments = arguments;
            this.Handler = handler;
            this.HelpLines = HelpTextHelper.GetHelpLines(this.Arguments.HelpText);
        }

        public ArgumentsBuilder Builder { get; } = new();

        public IArguments Arguments { get; }

        public string[] HelpLines { get; }

        public Func<IArguments, ValueTask<Result<TSuccess, ParserError<TError>>>> Handler { get; }
    }
}