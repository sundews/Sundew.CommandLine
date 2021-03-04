// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineBatcherTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.CommandLineBatcher
{
    using System.Linq;
    using FluentAssertions;
    using Sundew.Base.Computation;
    using Xunit;

    public class CommandLineBatcherTests
    {
        [Fact]
        public void Parse_Then_BatchArgumentsShouldBeAsExpected()
        {
            var commandLineParser = new CommandLineParser<int, int>();
            var batchArguments = commandLineParser.WithArguments(new BatchArguments(), arguments => Result.Success(0));

            var result = commandLineParser.Parse(@"-c ""git|tag -a {0}_{1} -m \""Release: {1} {0}\"""" ""git|push https://github.com {0}_{1}"" -v ""1.0.1"" Sundew.CommandLine");

            result.IsSuccess.Should().BeTrue();
            batchArguments.Commands.Should().BeEquivalentTo(
                new Command("git", @"tag -a {0}_{1} -m ""Release: {1} {0}"""),
                new Command("git", @"push https://github.com {0}_{1}"));
            batchArguments.Values!.SelectMany(x => x.Arguments).Should().Equal("1.0.1", "Sundew.CommandLine");
        }

        [Fact]
        public void Parse_When_ChoiceValueIsMissing_Then_BatchArgumentsShouldBeAsExpected()
        {
            var commandLineParser = new CommandLineParser<int, int>();
            commandLineParser.WithArguments(new BatchArguments(), arguments => Result.Success(0));

            var result = commandLineParser.Parse(string.Empty);

            result.IsSuccess.Should().BeFalse();
            result.Error.Type.Should().Be(ParserErrorType.RequiredArgumentMissing);
            result.Error.Message.Should().Be(@"-c/--commands
-v/--values or -vf/--values-files");
            result.Error.ToString().Should().Be(@"Error:
  The required options were missing:
   -c/--commands
   -v/--values or -vf/--values-files");
        }

        [Fact]
        public void CreateHelpText_Then_ResultShouldBeExpectedResult()
        {
            var commandLineParser = new CommandLineParser<int, int>();
            commandLineParser.WithArguments(new BatchArguments(), arguments => Result.Success(0));

            var result = commandLineParser.CreateHelpText();

            result.Should().Be(@$"Help
 Arguments:
  -c   | --commands              | The commands to be executed                                             | Required
                                   Format: ""{{command}}[|{{arguments}}]""...                                   
                                   Values can inject values by position with {{number}}                     
  -s   | --batch-value-separator | The batch value separator                                               | Default: |
  Values:                                                                                                  | Required
   -v  | --values                | The batches to be passed for each command                               | Default: [none]
                                   Each batch can contain multiple values separated by the batch separator
   -vf | --values-files          | A list of files containing batches                                      | Default: [none]
  -d   | --root-directory        | The directory to search for projects                                    | Default: Current directory
");
        }
    }
}