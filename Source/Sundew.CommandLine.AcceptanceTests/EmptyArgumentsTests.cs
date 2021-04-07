// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyArgumentsTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests
{
    using FluentAssertions;
    using Sundew.Base.Primitives.Computation;
    using Xunit;

    public class EmptyArgumentsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Given_a_purely_optional_commandline_parsing_null_or_an_empty_string_should_succeed(string commandLine)
        {
            var testee = new CommandLineParser<Args, int>();
            testee.WithArguments(new Args(), args => Result.Success(args));
            var result = testee.Parse(commandLine);

            result.Value.IsOn.Should().BeFalse();
        }

        private class Args : IArguments
        {
            public bool IsOn { get; private set; }

            public void Configure(IArgumentsBuilder argumentsBuilder)
            {
                argumentsBuilder.AddSwitch("s", "switch", this.IsOn, b => this.IsOn = b, string.Empty);
            }
        }
    }
}