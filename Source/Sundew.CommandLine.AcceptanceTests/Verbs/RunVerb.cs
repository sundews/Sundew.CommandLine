// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Verbs
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class RunVerb : IVerb
    {
        private readonly List<string> tasks;
        private readonly List<string> files;

        public RunVerb()
            : this(new List<string>(), 0, TimeSpan.FromSeconds(5), false, new List<string>())
        {
        }

        public RunVerb(List<string> tasks, int count, TimeSpan timeout, bool verbose, List<string> files)
        {
            this.files = files;
            this.Count = count;
            this.Timeout = timeout;
            this.Verbose = verbose;
            this.tasks = tasks;
        }

        public IVerb? NextVerb => null;

        public IReadOnlyList<string> Tasks => this.tasks;

        public int Count { get; private set; }

        public TimeSpan Timeout { get; private set; }

        public bool Verbose { get; private set; }

        public IReadOnlyList<string> Files => this.files;

        public string HelpText => "Runs the command with the arguments.";

        public string Name => "run";

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddRequiredList("t", "tasks", this.tasks, "The tasks to run", true);
            argumentsBuilder.AddRequired("c", "count", () => this.Count.ToString(CultureInfo.InvariantCulture), argument => this.Count = int.Parse(argument), "The amount to run");
            argumentsBuilder.AddOptional("to", "timeout", () => this.Timeout.ToString(), argument => this.Timeout = TimeSpan.Parse(argument), "The timeout");
            argumentsBuilder.AddSwitch("v", "verbose", this.Verbose, value => this.Verbose = value, "Use verbose logging");
            argumentsBuilder.AddOptionalValues("files", this.files, "The files to process", true);
        }
    }
}