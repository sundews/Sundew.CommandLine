// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerateParseTests.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests;

using System;
using System.Collections.Generic;
using FluentAssertions;
using Sundew.Base;
using Sundew.CommandLine.AcceptanceTests.Verbs;
using Xunit;

public class GenerateParseTests
{
    [Fact]
    public void Given_a_commandline_that_contains_options_switches_and_values_When_parsed_Then_parsedResult_should_match_expectedResult()
    {
        R.Success();
        var commandLineGenerator = new CommandLineGenerator();
        var expectedResult = new RunVerb(
            new List<string> { "Collect", "Build", "Compile", "Output" },
            4,
            TimeSpan.FromMinutes(3),
            true,
            new List<string> { @"c:\temp\file1.txt", @"c:\temp\file2.txt" });

        var commandLine = commandLineGenerator.Generate(expectedResult);

        var parsedResult = new RunVerb();
        var commandLineParser = new CommandLineParser<int, int>();
        commandLineParser.AddVerb(parsedResult, verb => R.Success(45));
        var result = commandLineParser.Parse(commandLine.Value);

        result.Value.Should().Be(45);
        parsedResult.Tasks.Should().Equal(expectedResult.Tasks);
        parsedResult.Verbose.Should().Be(expectedResult.Verbose);
        parsedResult.Timeout.Should().Be(expectedResult.Timeout);
        parsedResult.Count.Should().Be(expectedResult.Count);
        parsedResult.Files.Should().Equal(expectedResult.Files);
    }

    [Fact]
    public void Given_a_commandline_that_contains_a_single_item_option_followed_by_values_When_parsed_Then_parsedResult_should_match_expectedResult()
    {
        var commandLineGenerator = new CommandLineGenerator();
        var expectedResult = new RunGeneratorVerb(Mode.Dynamic, false, new List<string> { @"c:\temp\file.txt" });
        var commandLineResult = commandLineGenerator.Generate(expectedResult);

        var parsedResult = new RunGeneratorVerb();
        var commandLineParser = new CommandLineParser<int, int>();
        commandLineParser.AddVerb(parsedResult, runGeneratorVerb => R.Success(23));
        var parseResult = commandLineParser.Parse(commandLineResult.Value);

        parseResult.Value.Should().Be(23);
        parsedResult.AttachDebugger.Should().Be(expectedResult.AttachDebugger);
        parsedResult.Mode.Should().Be(expectedResult.Mode);
        parsedResult.Files.Should().Equal(expectedResult.Files);
    }
}