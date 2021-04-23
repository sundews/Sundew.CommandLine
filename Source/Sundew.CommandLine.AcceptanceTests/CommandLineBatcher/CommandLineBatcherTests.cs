// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineBatcherTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.CommandlineBatcher
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Sundew.Base.Primitives.Computation;
    using Xunit;

    public class CommandLineBatcherTests
    {
        [Fact]
        public void Parse_Then_BatchArgumentsShouldBeAsExpected()
        {
            var commandLineParser = new CommandLineParser<int, int>();
            var batchArguments = commandLineParser.WithArguments(new BatchArguments(), arguments => Result.Success(0));

            var result = commandLineParser.Parse(@"-c ""git|tag -a {0}_{1} -m \""Release: {1} {0}\"""" ""git|push https://github.com {0}_{1}"" -b ""1.0.1"" Sundew.CommandLine");

            result.IsSuccess.Should().BeTrue();
            batchArguments.Commands.Should().BeEquivalentTo(
                new Command("git", @"tag -a {0}_{1} -m ""Release: {1} {0}"""),
                new Command("git", @"push https://github.com {0}_{1}"));
            batchArguments.Batches!.SelectMany(x => x.Arguments).Should().Equal("1.0.1", "Sundew.CommandLine");
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
-b/--batches or -bf/--batches-files or -i/--stdio");
            result.Error.ToString().Should().Be(@"Error:
  The required options were missing:
   -c/--commands
   -b/--batches or -bf/--batches-files or -i/--stdio");
        }

        [Fact]
        public void CreateHelpText_Then_ResultShouldBeExpectedResult()
        {
            var commandLineParser = new CommandLineParser<int, int>();
            commandLineParser.WithArguments(new BatchArguments(), arguments => Result.Success(0));

            var result = commandLineParser.CreateHelpText();

            result.Should().Be(@$"Help
 Arguments:
  -c   | --commands              | The commands to be executed                                                                       | Required
                                   Format: ""[{{command}}][|{{arguments}}]""...
                                   Values can be injected by position with {{number}}
                                   If no command is specified, the argument is sent to standard output
  Batches with values                                                                                                                | Required
   -b  | --batches               | The batches to be passed for each command                                                         | Default: [none]
                                   Each batch can contain multiple values separated by the batch value separator
   -bf | --batches-files         | A list of files containing batches                                                                | Default: [none]
   -i  | --stdio                 | Indicates that batches should be read from standard input
  -bs  | --batch-separation      | Specifies how batches are separated:                                                              | Default: command-line
                                   [c]ommand-line, [n]ew-line, [w]indows-new-line, [u]nix-new-line
  -bvs | --batch-value-separator | The batch value separator                                                                         | Default: |
       | --if                    | A condition for each batch to check if it should run                                              | Default: [none]
                                   Format: [StringComparison:]{{lhs}} {{operator}} {{rhs}}
                                   lhs and rhs can be injected by position with {{number}}
                                   operators: == equals, |< starts with, >| ends with, >< contains
                                   negations: != not equals, !< not starts with, >! not ends with, <> not contains
                                   StringComparison: O Ordinal, OI OrdinalIgnoreCase, C CurrentCulture,
                                   CI CurrentCultureIgnoreCase, I InvariantCulture, II InvariantCultureIgnoreCase
  -d   | --root-directory        | The directory to search for projects                                                              | Default: Current directory
  -e   | --execution-order       | Specifies whether all commands are executed for the first [b]atch before moving to the next batch | Default: batch
                                   or the first [c]ommand is executed for all batches before moving to the next command
                                   - Finish first [b]atch first
                                   - Finish first [c]ommand first
  -mp  | --max-parallelism       | The degree of parallel execution (1-{Environment.ProcessorCount})                                                            | Default: 1
                                   Specify ""all"" for number of cores.
  -p   | --parallelize           | Specifies whether commands or batches run in parallel: [c]ommands, [b]atches                      | Default: commands
  -lv  | --logging-verbosity     | Logging verbosity: [n]ormal, [e]rrors, [q]uiet, [d]etailed                                        | Default: normal
");
        }
    }
}