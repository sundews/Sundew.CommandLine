// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidOptionTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests
{
    using FluentAssertions;
    using Sundew.Base.Primitives.Computation;
    using Xunit;

    public class InvalidOptionTests
    {
        [Fact]
        public void Parse_When_InvalidOptionIsSpecified_Then_ResultShouldBeFalseAndErrorShouldBeUnknownOption()
        {
            var commandLine = "-d";

            var testee = new CommandLineParser<int, int>();
            testee.WithArguments(new Arguments(), arguments => Result.Success(40));

            var result = testee.Parse(commandLine);

            result.IsSuccess.Should().BeFalse();
            result.Error.Type.Should().Be(ParserErrorType.UnknownOption);
            result.Error.Message.Should().Be(@"The option does not exist: -d");
        }

        private class Arguments : IArguments
        {
            public bool Switch { get; private set; }

            public void Configure(IArgumentsBuilder argumentsBuilder)
            {
                argumentsBuilder.AddSwitch("s", "switch", this.Switch, b => this.Switch = b, "A switch");
            }
        }
    }
}