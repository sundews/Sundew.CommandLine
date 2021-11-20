// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestsVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Verbs;

public class TestsVerb : IVerb
{
    public IVerb? NextVerb => null;

    public string HelpText { get; } = string.Empty;

    public string Name => "tests";

    public string? ShortName { get; } = null;

    public void Configure(IArgumentsBuilder argumentsBuilder)
    {
    }
}