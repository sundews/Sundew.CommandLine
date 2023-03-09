// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NestedVerbsTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests;

using FluentAssertions;
using Sundew.Base.Primitives.Computation;
using Sundew.CommandLine.AcceptanceTests.Verbs;
using Xunit;

public class NestedVerbsTests
{
    [Fact]
    public void Given_a_command_line_with_nested_verbs_then_result_should_be_expected_result()
    {
        var commandLineParser = new CommandLineParser<int, int>();
        const int ExpectedResult = 1;
        commandLineParser.AddVerb(
            new ExecuteVerb(),
            verb => R.Success(0),
            verbBuilder =>
            {
                verbBuilder.AddVerb(new TestsVerb(), verb => R.Success(ExpectedResult));
                verbBuilder.AddVerb(new BuildVerb(), verb => R.Success(2));
            });

        var result = commandLineParser.Parse("execute tests");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(ExpectedResult);
    }
}