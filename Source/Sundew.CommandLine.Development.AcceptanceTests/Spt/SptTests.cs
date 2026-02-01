// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SptTests.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Development.AcceptanceTests.Spt;

using AwesomeAssertions;
using Sundew.Base;

public class SptTests
{
    [Test]
    public void Given_CommandLineWithEmptyString_Then_ResultShouldContainEmptyString()
    {
        var commandLineParser = new CommandLineParser<int, int>();
        var updateVerb = commandLineParser.AddVerb(new UpdateVerb(), ExecuteAsync);
        var result = commandLineParser.Parse(@"update -s """" -pr");

        updateVerb.AllowPrerelease.Should().BeTrue();
        updateVerb.Source.Should().BeEmpty();
    }

    [Test]
    public void Given_DefaultArguments_When_CreatingHelpText_Then_ResultShouldBeExpectedHelp()
    {
        var commandLineParser = new CommandLineParser<int, int>();
        commandLineParser.AddVerb(new UpdateVerb(), ExecuteAsync);
        commandLineParser.AddVerb(new AwaitPublishVerb(), ExecuteAsync);
        commandLineParser.AddVerb(new PruneLocalSourceVerb(), v => R.Error(ParserError.From(-1)), builder =>
        {
            builder.AddVerb(new AllVerb(), ExecuteAsync);
        });
        commandLineParser.AddVerb(new DeleteVerb(), ExecuteAsync);

        var result = commandLineParser.CreateHelpText();

        result.Should().Be(@"Help
 Verbs:
   update/u                  Update package references
     -id  | --package-ids    | The package(s) to update. (* Wildcards supported)                         | Default: *
                               Format: Id[.Version] or ""Id[ Version]"" (Pinning version is optional)
     -p   | --projects       | The project(s) to update (* Wildcards supported)                          | Default: *
     -s   | --source         | The source or source name to search for packages (""All"" supported)        | Default: NuGet.config: defaultPushSource
          | --version        | The NuGet package version (* Wildcards supported).                        | Default: Latest version
     -d   | --root-directory | The directory to search for projects                                      | Default: Current directory
     -pr  | --prerelease     | Allow updating to latest prerelease version
     -v   | --verbose        | Verbose
     -l   | --local          | Forces the source to ""Local-Sundew""
     -sr  | --skip-restore   | Skips a dotnet restore command after package update.
   await/a                   Awaits the specified package to be published
     -s   | --source         | The source or source name to await publish                                | Default: NuGet.config: defaultPushSource
     -d   | --root-directory | The directory to search for projects                                      | Default: Current directory
     -t   | --timeout        | The wait timeout in seconds                                               | Default: 300
     -v   | --verbose        | Verbose
     <package-id>            | Specifies the package id and optionally the version                       | Required
                               Format: <PackageId>[.<Version>].
                               If the version is not provided, it must be specified by the version value
     <version>               | Specifies the NuGet Package version                                       | Default: [none]
   prune/p                   Prunes the matching packages for a local source
     all                     Prune the specified local source for all packages
       -p | --package-ids    | The packages to prune (* Wildcards supported)                             | Default: *
       -s | --source         | Local source or source name to search for packages                        | Default: Local-Sundew
       -v | --verbose        | Verbose
   delete/d                  Delete files
     -d   | --root-directory | The directory to search for projects                                      | Default: Current directory
     -r   | --recursive      | Specifies whether to recurse into subdirectories.
     -v   | --verbose        | Verbose
     <files>                 | The files to be deleted                                                   | Required
");
    }

    private static R<int, ParserError<int>> ExecuteAsync(object arg)
    {
        return R.Error(ParserError.From(-1));
    }
}