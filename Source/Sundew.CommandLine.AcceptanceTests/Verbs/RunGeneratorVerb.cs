// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunGeneratorVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Verbs;

using System.Collections.Generic;
using Sundew.Base.Primitives;

public enum Mode
{
    Static,

    Dynamic,
}

public class RunGeneratorVerb : IVerb
{
    private readonly List<string> files;

    public RunGeneratorVerb()
        : this(Mode.Static, false, new List<string>())
    {
    }

    public RunGeneratorVerb(Mode mode, bool attachDebugger, List<string> files)
    {
        this.Mode = mode;
        this.AttachDebugger = attachDebugger;
        this.files = files;
    }

    public IReadOnlyList<string> Files => this.files;

    public bool AttachDebugger { get; private set; }

    public Mode Mode { get; private set; }

    string IVerb.Name => "run";

    public string? ShortName { get; } = null;

    public IVerb? NextVerb => null;

    string IArguments.HelpText => "Runs the generator";

    public void Configure(IArgumentsBuilder argumentsBuilder)
    {
        argumentsBuilder.AddOptional("m", "mode", () => this.Mode.ToString(), s => this.Mode = s.ParseEnum<Mode>(), "The generator mode");
        argumentsBuilder.AddSwitch("d", "debug", this.AttachDebugger, value => this.AttachDebugger = value, "Attaches the debugger");
        argumentsBuilder.AddOptionalValues("files", this.files, "The files to process");
    }
}