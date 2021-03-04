// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal.Verbs
{
    internal class NullVerb : IVerb
    {
        public static readonly IVerb Instance = new NullVerb();

        private NullVerb()
        {
            this.NextVerb = default;
        }

        public IVerb? NextVerb { get; }

        public string Name { get; } = default!;

        public string? ShortName { get; } = null;

        public string HelpText { get; } = default!;

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
        }
    }
}