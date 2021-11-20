// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpuTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Spu;

using System;
using System.Collections.Generic;
using FluentAssertions;
using Sundew.Base.Primitives.Computation;
using Sundew.Base.Text;
using Xunit;

public class SpuTests
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
        const string ExpectedResult = @"-id Sundew.Base -p Sundew.CommandLine -s All --version 1.2.3.4 -d ""c:\with space""";
        var testee = new Arguments(new List<PackageId> { new("Sundew.Base", null) }, new List<string> { "Sundew.CommandLine" }, "All", new Version(1, 2, 3, 4), @"c:\with space", false);
        var commandLineGenerator = new CommandLineGenerator();

        var result = commandLineGenerator.Generate(testee);

        result.Value.Should().Be(ExpectedResult);
    }

    [Fact]
    public void Given_DefaultArguments_When_CreatingHelpText_Then_ResultShouldBeExpectedHelp()
    {
        const string ExpectedHelp = @"Help
 Arguments:              Runs a package update.
  -id | --package-ids    | The package(s) to update. (* Wildcards supported)                    | Default: Sundew.Base
                           Format: Id[.Version] or ""Id[ Version]"" (Pinning version is optional)
  -p  | --projects       | The project(s) to update (* Wildcards supported)                     | Default: Sundew.CommandLine
  -s  | --source         | The source or source name to search for packages (All supported)     | Default: NuGet.config: defaultPushSource
      | --version        | Pins the NuGet package version.                                      | Default: Latest if not pinned
  -d  | --root-directory | The directory to search to projects                                  | Default: Current directory
  -pr | --prerelease     | Allow updating to latest prerelease version
  -l  | --local          | Forces the source to ""Local-Sundew""
  -v  | --verbose        | Verbose
";
        var testee = new Arguments(new List<PackageId> { new("Sundew.Base", null) }, new List<string> { "Sundew.CommandLine" });
        var commandLineParser = new CommandLineParser<int, int>();
        commandLineParser.WithArguments(testee, arguments => Result.Success(0));

        var result = commandLineParser.CreateHelpText();

        result.Should().Be(ExpectedHelp);
    }

    [Fact]
    public void Given_DefaultArguments_When_ArgumentsAreSpecified_Then_DefaultArgumentsAreOverwritten()
    {
        const string ExpectedPackageId = "TransparentMoq";
        const string ExpectedProject = "Sundew.CommandLine.Tests";
        var testee = new Arguments(new List<PackageId> { new("Sundew.Base", null) }, new List<string> { "Sundew.CommandLine" });
        var commandLineParser = new CommandLineParser<int, int>();
        commandLineParser.WithArguments(testee, arguments => Result.Success(0));

        commandLineParser.Parse($"-id {ExpectedPackageId} -p {ExpectedProject}");

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
        testee.RootDirectory.Should().Be(ExpectedDirectory);
    }
}