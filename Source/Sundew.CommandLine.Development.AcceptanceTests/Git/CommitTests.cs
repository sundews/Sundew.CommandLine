// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommitTests.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Development.AcceptanceTests.Git;

using AwesomeAssertions;
using Sundew.Base;
using Sundew.Git.CommandLine;

public class CommitTests
{
    [Test]
    public void GenerateAndParse_Then_ResultShouldBeExpectedResult()
    {
        const string expectedMessage = @"message";
        var expectedResult = 0;

        var commandLineGenerator = new CommandLineGenerator();
        var commandLineParser = new CommandLineParser<int, int>();
        var push = commandLineParser.AddVerb(new Commit(), pushVerb => R.Success(expectedResult));

        var generateResult = commandLineGenerator.Generate(new Commit(expectedMessage), true);
        var parseResult = commandLineParser.Parse(generateResult.Value);

        parseResult.IsSuccess.Should().BeTrue();
        parseResult.Value.Should().Be(expectedResult);
        push.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void CreateHelpText_Then_ResultShouldBeExpectedResult()
    {
        const string expectedText = @"Help
 Verbs:
   commit/c          Record changes to the repository.
     -m | --message= | Use the given <msg> as the commit message.                                                                         | Default: [none]
     -q | --quiet    | Only print error and warning messages; all other output will be suppressed.
     -v | --verbose  | Show unified diff between the HEAD commit and what would be committed at the bottom of the commit message template
                       to help the user describe the commit by reminding what changes the commit has.
";
        var commandLineParser = new CommandLineParser<int, int>();
        commandLineParser.AddVerb(new Commit(), verb => R.Success(0));

        var result = commandLineParser.CreateHelpText();

        result.Should().Be(expectedText);
    }
}