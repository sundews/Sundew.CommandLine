// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Verbs
{
    public class BuildVerb : IVerb
    {
        public IVerb NextVerb => null;

        public string HelpText { get; }

        public string Name { get; } = "analyze";

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
        }
    }
}