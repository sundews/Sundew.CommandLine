// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineBatcherTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.CommandLineBatcher
{
    using FluentAssertions;
    using Sundew.Base.Computation;
    using Xunit;

    public class CommandLineBatcherTests
    {
        [Fact]
        public void Parse_Then_BatchArgumentsShouldBeAsExpected()
        {
            var commandLineParser = new CommandLineParser<int, int>();
            var batchArguments = commandLineParser.WithArguments(new BatcherArguments(), arguments => Result.Success(0));

            var result = commandLineParser.Parse(@"-c ""git|tag -a {0}_{1} -m \""Release: {1} {0}\"""" ""git|push https://github.com {0}_{1}"" -v ""1.0.1"" Sundew.CommandLine");

            result.IsSuccess.Should().BeTrue();
            batchArguments.Commands.Should().BeEquivalentTo(
                new Command("git", @"tag -a {0}_{1} -m ""Release: {1} {0}"""),
                new Command("git", @"push https://github.com {0}_{1}"));
            batchArguments.Values.Should().Equal("1.0.1", "Sundew.CommandLine");
        }
    }
}