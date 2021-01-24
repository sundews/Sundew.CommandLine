// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Verbs
{
    public class ExecuteVerb : IVerb
    {
        private readonly TestsVerb testsVerb;

        public ExecuteVerb()
        {
            this.testsVerb = default!;
        }

        public ExecuteVerb(TestsVerb testsVerb)
        {
            this.testsVerb = testsVerb;
        }

        public IVerb? NextVerb => null;

        public string Name { get; } = "execute";

        public string HelpText { get; } = string.Empty;

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
        }
    }
}