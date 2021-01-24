// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SbuTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Sbu
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Sundew.Base.Computation;
    using Sundew.Base.Text;
    using Xunit;

    public class SbuTests
    {
        [Fact]
        public void Given_NoValues_Then_ResultShouldBeEmpty()
        {
            var testee = new Arguments(new List<PackageId>(), new List<string>(), null, null, null, false);
            var commandLineGenerator = new CommandLineGenerator();

            var result = commandLineGenerator.Generate(testee);

            result.Value.Should().Be(Strings.Empty);
        }

        [Fact]
        public void Given_ManyValues_Then_ResultShouldBeExpectedResult()
        {
            const string ExpectedResult = @"-id ""Sundew.Base"" -pn Sundew.CommandLine -s All -v 1.2.3.4 -d ""c:\with space""";
            var testee = new Arguments(new List<PackageId> { new("Sundew.Base", null) }, new List<string> { "Sundew.CommandLine" }, "All", new Version(1, 2, 3, 4), @"c:\with space", false);
            var commandLineGenerator = new CommandLineGenerator();

            var result = commandLineGenerator.Generate(testee);

            result.Value.Should().Be(ExpectedResult);
        }

        [Fact]
        public void Given_DefaultArguments_When_ArgumentsAreSpecified_Then_DefaultArgumentsAreOverwritten()
        {
            const string ExpectedPackageId = "TransparentMoq";
            const string ExpectedProject = "Sundew.CommandLine.Tests";
            var testee = new Arguments(new List<PackageId> { new("Sundew.Base", null) }, new List<string> { "Sundew.CommandLine" });
            var commandLineParser = new CommandLineParser<int, int>();
            commandLineParser.WithArguments(testee, arguments => Result.Success(0));

            commandLineParser.Parse($"-id {ExpectedPackageId} -pn {ExpectedProject}");

            testee.PackageIds.Should().Equal(new[] { new PackageId(ExpectedPackageId, null) });
            testee.Projects.Should().Equal(new[] { ExpectedProject });
        }

        [Fact]
        public void Given_DefaultArguments_When_ArgumentsNotAreSpecifies_Then_DefaultArgumentsUsed()
        {
            const string ExpectedDirectory = @"c:\with space";
            const string ExpectedPackageId = "Sundew.Base";
            const string ExpectedProjectName = "Sundew.CommandLine";
            var testee = new Arguments(new List<PackageId> { new(ExpectedPackageId, null) }, new List<string> { ExpectedProjectName });
            var commandLineParser = new CommandLineParser<int, int>();
            commandLineParser.WithArguments(testee, arguments => Result.Success(0));

            commandLineParser.Parse($@"-d ""{ExpectedDirectory}""");

            testee.PackageIds.Should().Equal(new[] { new PackageId(ExpectedPackageId, null) });
            testee.Projects.Should().Equal(new[] { ExpectedProjectName });
            testee.RootDictionary.Should().Be(ExpectedDirectory);
        }
    }
}