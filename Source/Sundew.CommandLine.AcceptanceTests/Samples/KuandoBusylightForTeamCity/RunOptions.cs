// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunOptions.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Samples.KuandoBusylightForTeamCity
{
    using System;
    using System.Collections.Generic;
    using Sundew.Base.Primitives.Numeric;

    public class RunOptions : IArguments, IRunOptions
    {
        private readonly List<string> hidDeviceIds;

        public RunOptions()
        : this(default!, default!)
        {
        }

        public RunOptions(string hostName, string buildTypeId, CredentialOptions? credentials = null, TimeSpan? refreshInterval = null, List<string>? hidDeviceIds = null)
        {
            this.HostName = hostName;
            this.BuildTypeId = buildTypeId;
            this.Credentials = credentials;
            this.RefreshInterval = refreshInterval ?? TimeSpan.FromMilliseconds(1000);
            this.hidDeviceIds = hidDeviceIds ?? new List<string>();
        }

        public string HostName { get; private set; }

        public string BuildTypeId { get; private set; }

        public CredentialOptions? Credentials { get; private set; }

        ICredentials? IRunOptions.Credentials => this.Credentials;

        public TimeSpan RefreshInterval { get; private set; }

        public IReadOnlyList<string> HidDeviceIds => this.hidDeviceIds;

        public string HelpText { get; } = "Runs something";

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddRequired("h", "host-name", () => this.HostName, hostName => this.HostName = hostName, "Specifies the TeamCity host name");
            argumentsBuilder.AddRequired("b", "build-type-id", () => this.BuildTypeId, buildId => this.BuildTypeId = buildId, "Specifies the TeamCity build type id");
            argumentsBuilder.AddOptional("c", "credentials", this.Credentials, () => new CredentialOptions(), options => this.Credentials = options, "Specifies the credentials to connect to TeamCity");
            var refreshIntervalRange = new Interval<TimeSpan>(TimeSpan.FromMilliseconds(200), TimeSpan.FromMinutes(10));
            argumentsBuilder.AddOptional(
                "ri",
                "refresh-interval",
                () => refreshIntervalRange.Limit(this.RefreshInterval).ToString(),
                value => this.RefreshInterval = refreshIntervalRange.Limit(TimeSpan.Parse(value)),
                $"The refresh interval within the {refreshIntervalRange}");
            argumentsBuilder.AddOptionalList("d", "devices", this.hidDeviceIds, "The list of hid device ids", true);
        }
    }
}