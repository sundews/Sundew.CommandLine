// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PushTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Git
{
    using FluentAssertions;
    using Sundew.Base.Computation;
    using Sundew.Git.CommandLine;
    using Xunit;

    public class PushTests
    {
        [Fact]
        public void GenerateAndParse_Then_ResultShouldBeExpectedResult()
        {
            const string expectedRepository = @"repository";
            const string expectedRefspec = @"refspec";
            var expectedResult = 0;

            var commandLineGenerator = new CommandLineGenerator();
            var commandLineParser = new CommandLineParser<int, int>();
            var push = commandLineParser.AddVerb(new Push(), pushVerb => Result.Success(expectedResult));

            var generateResult = commandLineGenerator.Generate(new Push(new Repository(expectedRepository, new Refspec(expectedRefspec))));
            var parseResult = commandLineParser.Parse(generateResult.Value);

            parseResult.IsSuccess.Should().BeTrue();
            parseResult.Value.Should().Be(expectedResult);
            push.Repository.Name.Should().Be(expectedRepository);
            push.Repository.Refspec.ToString().Should().Be(expectedRefspec);
        }

        [Fact]
        public void CreateHelpText_Then_ResultShouldBeExpectedResult()
        {
            const string expectedText = @"Help
 Verbs:
   push | Update remote refs along with associated objects.
     -o | --push-option=  | Transmit the given string to the server, which passes them to the pre-receive as well as the post-receive hook. | Default: [none]
     -v | --verbose       | Show unified diff between the HEAD commit and what would be committed at the bottom of the commit message template to help the user describe the commit by reminding what changes the commit has.
     <repository refspec> | The repository and refspec. | Default: [none]
";
            var commandLineParser = new CommandLineParser<int, int>();
            commandLineParser.AddVerb(new Push(), verb => Result.Success(0));

            var result = commandLineParser.CreateHelpText();

            result.Should().Be(expectedText);
        }
    }
}