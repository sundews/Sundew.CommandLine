// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensionTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.UnitTests.Internals
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Sundew.CommandLine.Internal.Extensions;
    using Xunit;

    public class StringExtensionTests
    {
        [Fact]
        public void SplitBasedCommandLineTokenizer_Then_QuotesAreNotProperlyTerminated_Then_ResultShouldContainRemainderInLastElement()
        {
            var commandLine = $@"-fl --max-size ""4000 -cl";

            var result = commandLine.AsMemory().SplitBasedCommandLineTokenizer().ToArray();

            result.Should().Equal("-fl", "--max-size", "4000 -cl");
        }
    }
}