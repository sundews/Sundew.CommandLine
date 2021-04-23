// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LfsTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Git.LargeFileSystem
{
    using FluentAssertions;
    using Sundew.Base.Primitives.Computation;
    using Sundew.Git.CommandLine.LargeFileSystem;
    using Xunit;

    public class LfsTests
    {
        [Fact]
        public void GenerateAndParse_Then_ResultShouldBeExpectedResult()
        {
            const string expectedPattern = "*.*";
            const int expectedResult = 2;
            var commandLineGenerator = new CommandLineGenerator();
            var commandLineParser = new CommandLineParser<int, int>();
            Track? track = null;
            commandLineParser.AddVerb(new Lfs(), lfsVerb => Result.Success(0), builder =>
            {
                builder.AddVerb(new Install(), install => Result.Success(1));
                track = builder.AddVerb(new Track(), trackVerb => Result.Success(expectedResult));
                builder.AddVerb(new Untrack(), untrack => Result.Success(3));
            });

            var generateResult = commandLineGenerator.Generate(new Lfs(new Track(expectedPattern)));
            var parseResult = commandLineParser.Parse(generateResult.Value);

            parseResult.IsSuccess.Should().BeTrue();
            parseResult.Value.Should().Be(expectedResult);
            track?.Pattern.Should().Be(expectedPattern);
        }

        [Fact]
        public void Parse_When_VerbIsUnknown_Then_ResultShouldBeExpectedResult()
        {
            var commandLineParser = CreateParser();

            var parseResult = commandLineParser.Parse("lfs adsldasdas");

            parseResult.IsSuccess.Should().BeFalse();
            parseResult.Error.Type.Should().Be(ParserErrorType.UnknownVerb);
        }

        [Fact]
        public void CreateHelpText_Then_ResultShouldBeExpectedResult()
        {
            const string expectedResult = @"Help
 Verbs:
   lfs           | Git Large File Storage (LFS) replaces large files.
     install     | Installs lfs into git hooks
     track       | Start tracking the given patterns(s) through Git LFS.
       <pattern> | The <pattern> argument is written to .gitattributes.   | Default: [none]
     untrack     | Stop tracking the given patterns(s) through Git LFS.
       <pattern> | The <pattern> argument is removed from .gitattributes. | Default: [none]
";
            var commandLineParser = CreateParser();

            var result = commandLineParser.CreateHelpText();

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("-?")]
        [InlineData("?")]
        [InlineData("-h")]
        [InlineData("--help")]
        public void Parse_When_HelpIsRequested_Then_ResultShouldBeExpectedResult(string helpArgument)
        {
            const string expectedHelpText = @"Help
 track       | Start tracking the given patterns(s) through Git LFS.
   <pattern> | The <pattern> argument is written to .gitattributes. | Default: [none]
";
            var commandLineParser = CreateParser();

            var parseResult = commandLineParser.Parse($"lfs track {helpArgument}");

            parseResult.IsSuccess.Should().BeFalse();
            parseResult.Error.Type.Should().Be(ParserErrorType.HelpRequested);
            parseResult.Error.Message.Should().Be(expectedHelpText);
        }

        private static CommandLineParser<int, int> CreateParser()
        {
            var commandLineParser = new CommandLineParser<int, int>();
            commandLineParser.AddVerb(new Lfs(), lfsVerb => Result.Success(0), builder =>
            {
                builder.AddVerb(new Install(), install => Result.Success(1));
                builder.AddVerb(new Track(), track => Result.Success(2));
                builder.AddVerb(new Untrack(), untrack => Result.Success(3));
            });
            return commandLineParser;
        }
    }
}