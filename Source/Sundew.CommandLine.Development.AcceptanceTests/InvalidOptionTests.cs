// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidOptionTests.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Development.AcceptanceTests;

using AwesomeAssertions;
using Sundew.Base;

public class InvalidOptionTests
{
    [Test]
    public void Parse_When_InvalidOptionIsSpecified_Then_ResultShouldBeFalseAndErrorShouldBeUnknownOption()
    {
        var commandLine = "-d";

        var testee = new CommandLineParser<int, int>();
        testee.WithArguments(new Arguments(), arguments => R.Success(40));

        var result = testee.Parse(commandLine);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Type.Should().Be(ParserErrorType.UnknownOption);
        result.Error.Message.Should().Be(@"The option does not exist: -d");
    }

    [Test]
    public void Parse_When_UnknownOptionIsSpecified_Then_ResultShouldBeFalseAndErrorShouldBeUnknownOption()
    {
        var commandLine = "-n Hi -d 43";

        var testee = new CommandLineParser<int, int>();
        testee.WithArguments(new Arguments(), arguments => R.Success(40));

        var result = testee.Parse(commandLine);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Type.Should().Be(ParserErrorType.UnknownOption);
        result.Error.Message.Should().Be(@"The option does not exist: -d");
    }

    private class Arguments : IArguments
    {
        public bool Switch { get; private set; }

        public string Name { get; private set; } = "name";

        public string Value { get; private set; } = "default";

        public string HelpText { get; } = "none";

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddRequired("n", "name", () => this.Name, s => this.Name = s, "A name");

            argumentsBuilder.AddRequired("v", "value", () => this.Value, s => this.Value = s, "A value");

            argumentsBuilder.AddSwitch("s", "switch", this.Switch, b => this.Switch = b, "A switch");
        }
    }
}