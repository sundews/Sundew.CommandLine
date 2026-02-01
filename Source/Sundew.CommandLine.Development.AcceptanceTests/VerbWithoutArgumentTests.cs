// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerbWithoutArgumentTests.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Development.AcceptanceTests;

using AwesomeAssertions;
using Sundew.Base;
using Sundew.CommandLine.Development.AcceptanceTests.Verbs;

public class VerbWithoutArgumentTests
{
    [Test]
    public void Given_a_commandline_that_only_contains_verbs_When_parsed_with_a_verb_that_ignores_values_Then_parsing_should_fail_with_verb_not_registered()
    {
        const int expectedResult = 97;
        var commandLineParser = new CommandLineParser<int, int>();
        commandLineParser.AddVerb(new ParameterLessVerb(), parameterLessVerb => R.Success(expectedResult));

        var result = commandLineParser.Parse(new[] { "run", "and", "fail" });

        result.IsSuccess.Should().BeFalse();
        result.Error!.Type.Should().Be(ParserErrorType.UnknownVerb);
    }
}