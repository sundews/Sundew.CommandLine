// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitTests.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Git;

using FluentAssertions;
using Sundew.Base;
using Sundew.Git.CommandLine;
using Xunit;

public class InitTests
{
    [Fact]
    public void GenerateAndParse_Then_ResultShouldBeExpectedResult()
    {
        const string expectedDirectory = @"c:\test with space";
        const string expectedTemplateDirectory = @"c:\template with space";
        var expectedResult = 0;

        var commandLineGenerator = new CommandLineGenerator();
        var commandLineParser = new CommandLineParser<int, int>();
        var init = commandLineParser.AddVerb(new Init(), initVerb => R.Success(expectedResult));

        var generateResult = commandLineGenerator.Generate(new Init(expectedDirectory)
        {
            TemplateDirectory = expectedTemplateDirectory,
        });
        var parseResult = commandLineParser.Parse(generateResult.Value);

        parseResult.IsSuccess.Should().BeTrue();
        parseResult.Value.Should().Be(expectedResult);
        init.TemplateDirectory.Should().Be(expectedTemplateDirectory);
        init.DirectoryPath.Should().Be(expectedDirectory);
    }
}