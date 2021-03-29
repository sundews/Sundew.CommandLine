// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidConfigurationTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests
{
    using FluentAssertions;
    using Sundew.Base.Primitives.Computation;
    using Xunit;

    public class InvalidConfigurationTests
    {
        [Fact]
        public void Parse_When_NoArgumentsNorVerbsAreConfigured_Then_ResultIsSuccessShouldBeFalse()
        {
            var testee = new CommandLineParser<int, int>();

            var result = testee.Parse("test");

            result.IsSuccess.Should().BeFalse();
            result.Error.Type.Should().Be(ParserErrorType.ArgumentsAndVerbsAreNotConfigured);
        }

        [Fact]
        public void Parse_When_VerbsAreConfiguredButNoArgumentsAndArgumentIsNotAVerb_Then_ResultIsSuccessShouldBeFalse()
        {
            var testee = new CommandLineParser<int, int>();
            testee.AddVerb(new Verbs.BuildVerb(), verb => Result.Success(0));

            var result = testee.Parse("test");

            result.IsSuccess.Should().BeFalse();
            result.Error.Type.Should().Be(ParserErrorType.ArgumentsNotConfiguredOrUnknownVerb);
        }
    }
}