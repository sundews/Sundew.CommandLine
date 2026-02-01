// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgramVerb.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Development.AcceptanceTests.Verbs;

public class ProgramVerb : IVerb
{
    public IVerb? NextVerb => null;

    public string HelpText { get; } = string.Empty;

    public string Name { get; } = "program";

    public string? ShortName { get; } = null;

    public void Configure(IArgumentsBuilder argumentsBuilder)
    {
    }
}