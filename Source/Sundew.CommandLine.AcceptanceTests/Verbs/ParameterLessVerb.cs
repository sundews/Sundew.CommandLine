// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterLessVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Verbs
{
    public class ParameterLessVerb : IVerb
    {
        public IVerb? NextVerb => null;

        public string HelpText => "Runs something";

        public string Name => "run";

        public string? ShortName { get; } = null;

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
        }
    }
}