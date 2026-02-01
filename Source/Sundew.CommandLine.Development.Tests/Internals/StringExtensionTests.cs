// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensionTests.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Development.Tests.Internals;

using System;
using System.Linq;
using AwesomeAssertions;
using Sundew.CommandLine.Extensions;

public class StringExtensionTests
{
    [Test]
    public void SplitBasedCommandLineTokenizer_When_QuotesAreNotProperlyTerminated_Then_ResultShouldContainRemainderInLastElement()
    {
        var commandLine = $@"-fl --max-size ""4000 -cl";

        var result = commandLine.AsMemory().ParseCommandLineArguments().ToArray();

        result.Select(x => x.ToString()).Should().Equal("-fl", "--max-size", "4000 -cl");
    }

    [Test]
    public void SplitBasedCommandLineTokenizer_When_TextContainsAdditionalSpace_Then_ResultShouldBeProperlyTokenized()
    {
        var commandLine = $@"-fl  max-size -cl";

        var result = commandLine.AsMemory().ParseCommandLineArguments().ToArray();

        result.Select(x => x.ToString()).Should().Equal("-fl", "max-size", "-cl");
    }
}