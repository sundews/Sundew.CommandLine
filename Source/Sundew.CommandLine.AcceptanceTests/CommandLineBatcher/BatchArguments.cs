// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BatchArguments.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.CommandLineBatcher
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Sundew.Base.Collections;
    using Sundew.Base.Text;
    using Sundew.CommandLine;

    public class BatchArguments : IArguments
    {
        private readonly List<Command> commands;
        private readonly List<Values>? values;
        private readonly List<string>? valuesFiles;

        public BatchArguments(List<Command> commands, List<Values>? values = null, List<string>? valuesFiles = null, string? rootDirectory = null, string? batchSeparator = null)
        {
            this.commands = commands;
            this.values = values;
            this.valuesFiles = valuesFiles;
            this.RootDirectory = rootDirectory ?? Directory.GetCurrentDirectory();
            this.BatchValueSeparator = batchSeparator ?? "|";
        }

        public BatchArguments()
            : this(new List<Command>(), new List<Values>(), new List<string>())
        {
        }

        public IReadOnlyList<Command> Commands => this.commands;

        public string RootDirectory { get; private set; }

        public IReadOnlyList<string>? ValuesFiles => this.valuesFiles;

        public IReadOnlyList<Values>? Values => this.values;

        public string BatchValueSeparator { get; private set; }

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.OptionsHelpOrder = OptionsHelpOrder.AsAdded;
            argumentsBuilder.AddRequiredList("c", "commands", this.commands, this.SerializeCommand, this.DeserializeCommand, @$"The commands to be executed{Environment.NewLine}Format: ""{{command}}[|{{arguments}}]""...{Environment.NewLine}Values can inject values by position with {{number}}", true);
            argumentsBuilder.AddOptional("s", "batch-value-separator", () => this.BatchValueSeparator, s => this.BatchValueSeparator = s, "The batch value separator");
            argumentsBuilder.RequireAnyOf("Values:", x => x
                .AddList("v", "values", this.values!, this.SerializeBatch, this.DeserializeBatch, $"The batches to be passed for each command{Environment.NewLine}Each batch can contain multiple values separated by the batch separator", true)
                .AddList("vf", "values-files", this.valuesFiles!, "A list of files containing batches", true));
            argumentsBuilder.AddOptional("d", "root-directory", () => this.RootDirectory, s => this.RootDirectory = s, "The directory to search for projects", true, defaultValueText: "Current directory");
        }

        private Command DeserializeCommand(string arg1, CultureInfo arg2)
        {
            var args = arg1.Split('|', StringSplitOptions.RemoveEmptyEntries);
            return args.Length switch
            {
                1 => new Command(args[0], string.Empty),
                2 => new Command(args[0], args[1]),
                _ => throw new ArgumentException(@$"Argument {arg1} did not follow the format ""{{command}}[|{{arguments}}]""..."),
            };
        }

        private string SerializeCommand(Command arg1, CultureInfo arg2)
        {
            return $"{arg1.Executable}|{arg1.Arguments}";
        }

        private Values DeserializeBatch(string arg1, CultureInfo arg2)
        {
            return CommandLineBatcher.Values.From(arg1, this.BatchValueSeparator!);
        }

        private string SerializeBatch(Values arg1, CultureInfo arg2)
        {
            return arg1.Arguments.AggregateToStringBuilder(
                (builder, s) => builder.Append(s).Append(this.BatchValueSeparator),
                builder => builder.ToStringFromEnd(1));
        }
    }
}