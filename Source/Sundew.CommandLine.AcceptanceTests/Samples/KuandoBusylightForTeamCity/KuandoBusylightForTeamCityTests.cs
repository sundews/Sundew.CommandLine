// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KuandoBusylightForTeamCityTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Samples.KuandoBusylightForTeamCity
{
    using System.Linq;
    using FluentAssertions;
    using Sundew.Base.Computation;
    using Xunit;

    public class KuandoBusylightForTeamCityTests
    {
        [Fact]
        public void Given_a_complete_commandline_Then_parsed_result_should_be_expected_values()
        {
            const string expectedHost = "SomeBuildTypeId";
            const string expectedBuildTypeId = "SomeBuildTypeId";
            const string expectedUserName = "user";
            const string expectedPassword = "pwd";
            const string expectedDevice =
                @"\\?\hid#vid_27bb&pid_3bcd#6&5ed6887&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}";
            var commandLine = $@"-h {expectedHost} -b {expectedBuildTypeId} -c -u {expectedUserName} -p {expectedPassword} -d ""\{expectedDevice}""";
            var testee = new CommandLineParser<RunOptions, int>();
            testee.WithArguments(new RunOptions(null, null), options => Result.Success(options));

            var result = testee.Parse(commandLine);

            result.IsSuccess.Should().BeTrue();
            result.Value.HostName.Should().Be(expectedHost);
            result.Value.BuildTypeId.Should().Be(expectedBuildTypeId);
            result.Value.Credentials.UserName.Should().Be(expectedUserName);
            result.Value.Credentials.Password.Should().Be(expectedPassword);
            result.Value.HidDeviceIds.First().Should().Be(expectedDevice);
        }
    }
}