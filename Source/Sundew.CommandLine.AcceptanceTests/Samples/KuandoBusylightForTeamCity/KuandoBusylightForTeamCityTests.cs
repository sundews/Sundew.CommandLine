// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KuandoBusylightForTeamCityTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Samples.KuandoBusylightForTeamCity;

using System.Net;
using FluentAssertions;
using Sundew.Base.Primitives.Computation;
using Xunit;

public class KuandoBusylightForTeamCityTests
{
    private const string ExpectedHost = "SomeBuildTypeId";
    private const string ExpectedBuildTypeId = "SomeBuildTypeId";
    private const string ExpectedUserName = "user";
    private const string ExpectedPassword = "pwd";
    private const string ExpectedDevice = @"\\?\hid#vid_27bb&pid_3bcd#6&5ed6887&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}";

    [Fact]
    public void Given_a_complete_commandline_Then_parsed_result_should_be_expected_values()
    {
        var commandLine = $@"-h {ExpectedHost} -b {ExpectedBuildTypeId} -c -u {ExpectedUserName} -p {ExpectedPassword} -d ""\{ExpectedDevice}""";
        var testee = new CommandLineParser<RunOptions, int>();
        testee.WithArguments(new RunOptions(), options => R.Success(options));

        var result = testee.Parse(commandLine);

        result.IsSuccess.Should().BeTrue();
        result.Value!.HostName.Should().Be(ExpectedHost);
        result.Value.BuildTypeId.Should().Be(ExpectedBuildTypeId);
        result.Value.Credentials?.UserName.Should().Be(ExpectedUserName);
        new NetworkCredential(string.Empty, result.Value.Credentials?.Password).Password.Should().Be(ExpectedPassword);
        result.Value.HidDeviceIds[0].Should().Be(ExpectedDevice);
    }

    [Fact]
    public void Given_a_configured_parser_Then_result_should_be_expected_values()
    {
        const string expectedHelpText = @"Help
 Arguments:                Runs something
  -h  | --host-name        | Specifies the TeamCity host name                                               | Required
  -b  | --build-type-id    | Specifies the TeamCity build type id                                           | Required
  -c  | --credentials      | Specifies the credentials to connect to TeamCity                               | Default: [none]
    -u | --username         | Specifies the username                                                         | Required
    -p | --password         | Specifies the password                                                         | Required
  -ri | --refresh-interval | The refresh interval within the Interval: min: 00:00:00.2000000, max: 00:10:00 | Default: 00:00:01
  -d  | --devices          | The list of hid device ids                                                     | Default: [none]
";

        var testee = new CommandLineParser<RunOptions, int>();
        testee.WithArguments(new RunOptions(), options => R.Success(options));

        var result = testee.CreateHelpText();

        result.Should().Be(expectedHelpText);
    }

    [Fact]
    public void Given_a_configured_parser_When_parse_is_called_Then_help_text_should_not_differ()
    {
        var commandLine = $@"-h {ExpectedHost} -b {ExpectedBuildTypeId} -c -u {ExpectedUserName} -p {ExpectedPassword} -d ""\{ExpectedDevice}""";
        var testee = new CommandLineParser<RunOptions, int>();
        testee.WithArguments(new RunOptions(), options => R.Success(options));
        var expectedResult = testee.CreateHelpText();
        testee.Parse(commandLine);

        var result = testee.CreateHelpText();

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(true, 0)]
    [InlineData(false, 1)]
    public void Given_a_configured_parser_Then_help_text_should_not_differ(bool isSuccess, int expectedResult)
    {
        var commandLine = $@"-h {ExpectedHost} -b {ExpectedBuildTypeId} -c -u {ExpectedUserName} -p {ExpectedPassword} -d ""\{ExpectedDevice}""";
        var testee = new CommandLineParser<int, int>();
        testee.WithArguments(new RunOptions(), options =>
        {
            if (isSuccess)
            {
                return R.Success(0);
            }

            return R.Error(new ParserError<int>(1));
        });
        var parserResult = testee.Parse(commandLine);

        var result = parserResult.GetExitCode();

        result.Should().Be(expectedResult);
    }
}