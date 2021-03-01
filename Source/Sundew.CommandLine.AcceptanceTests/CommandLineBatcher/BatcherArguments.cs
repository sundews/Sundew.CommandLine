// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BatcherArguments.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.CommandLineBatcher
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Sundew.Base.Collections;
    using Sundew.Base.Text;
    using Sundew.CommandLine;

    public class BatcherArguments : IArguments
    {
        private readonly List<string> values;
        private readonly List<Command> commands;

        public BatcherArguments(List<Command> commands, List<string> values, string? batchSeparator = null)
        {
            this.values = values;
            this.BatchSeparator = batchSeparator ?? "|";
            this.commands = commands;
        }

        public BatcherArguments()
            : this(new List<Command>(), new List<string>())
        {
        }

        public string? BatchSeparator { get; private set; }

        public IReadOnlyList<Command> Commands => this.commands;

        public IReadOnlyList<string> Values => this.values;

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddRequiredList("c", "commands", this.commands, this.SerializeCommand, this.DeserializeCommand, @$"The commands to be executed{Environment.NewLine}Format: ""{{command}}|{{arguments}}""[ ""{{command}}|{{arguments}}]*""{Environment.NewLine}Values can inject values by position with {{number}}", true);
            argumentsBuilder.AddOptional("s", "batch-separator", () => this.BatchSeparator, s => this.BatchSeparator = s, "The batch separator");
            argumentsBuilder.AddRequiredList("v", "values", this.values, "The values to be passed per command", true);
        }

        private Command DeserializeCommand(string arg1, CultureInfo arg2)
        {
            var args = arg1.Split('|');
            return new Command(args[0], args[1]);
        }

        private string SerializeCommand(Command arg1, CultureInfo arg2)
        {
            return $"{arg1.Executable}|{arg1.Arguments}";
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class Command
#pragma warning restore SA1402 // File may only contain a single type
    {
        public Command(string executable, string arguments)
        {
            this.Executable = executable;
            this.Arguments = arguments;
        }

        public string Executable { get; }

        public string Arguments { get; }
    }
}