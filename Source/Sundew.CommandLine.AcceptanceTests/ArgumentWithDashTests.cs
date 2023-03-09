// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentWithDashTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests;

using System;
using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using Sundew.Base.Primitives.Computation;
using Xunit;

public class ArgumentWithDashTests
{
    [Theory]
    [InlineData(-3)]
    [InlineData(2)]
    public void Given_a_commandline_with_an_argument_starting_with_dash_Then_result_should_be_expectedResult(int expectedNumber)
    {
        var commandLine = $@"-n ""{(expectedNumber < 0 ? "\\" : string.Empty)}{expectedNumber}""";
        var testee = new CommandLineParser<NumberOptions, int>();
        testee.WithArguments(new NumberOptions(0), x => R.Success(x));

        var result = testee.Parse(commandLine);

        result.Value.Number.Should().Be(expectedNumber);
    }

    [Fact]
    public void Given_argument_with_dashed_value_Then_result_should_be_expected_result()
    {
        var expectedResult = $@"-n \-3";
        var argument = new NumberOptions(-3);
        var testee = new CommandLineGenerator();

        var result = testee.Generate(argument);

        result.Value.Should().Be(expectedResult);
    }

    [Fact]
    public void Given_a_commandline_with_arguments_starting_with_dashes_Then_result_should_be_expected_result()
    {
        var commandLine = $@"-n \-3 4 \-7";
        var testee = new CommandLineParser<NumbersOptions, int>();
        testee.WithArguments(new NumbersOptions(), x => R.Success(x));

        var result = testee.Parse(commandLine);

        result.Value.Numbers.Should().Equal(-3, 4, -7);
    }

    [Fact]
    public void Given_arguments_with_dashed_values_Then_result_should_be_expected_result()
    {
        var expectedResult = $@"-n \-3 4 \-7";
        var arguments = new NumbersOptions();
        arguments.Numbers.AddRange(new[] { -3, 4, -7 });
        var testee = new CommandLineGenerator();

        var result = testee.Generate(arguments);

        result.Value.Should().Be(expectedResult);
    }

    private class NumberOptions : IArguments
    {
        public NumberOptions(int number)
        {
            this.Number = number;
        }

        public int Number { get; private set; }

        public string HelpText { get; } = "Numbers";

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddRequired("n", "number", ci => this.Number.ToString(ci).AsSpan(), (x, ci) => this.Number = int.Parse(x, NumberStyles.Integer, ci), "A number");
        }
    }

    private class NumbersOptions : IArguments
    {
        public List<int> Numbers { get; } = new List<int>();

        public string HelpText { get; } = "Numbers";

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddRequiredList("n", "numbers", this.Numbers, (x, ci) => x.ToString(ci), int.Parse, "Numbers");
        }
    }
}