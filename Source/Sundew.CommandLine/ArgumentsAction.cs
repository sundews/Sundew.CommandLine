// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentsAction.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    using System;
    using Sundew.Base.Computation;

    internal class ArgumentsAction<TSuccess, TError>
    {
        public ArgumentsAction(IArguments arguments, Func<IArguments, Result<TSuccess, ParserError<TError>>> handler)
        {
            this.Arguments = arguments;
            this.Handler = handler;
        }

        public IArguments Arguments { get; }

        public Func<IArguments, Result<TSuccess, ParserError<TError>>> Handler { get; }
    }
}