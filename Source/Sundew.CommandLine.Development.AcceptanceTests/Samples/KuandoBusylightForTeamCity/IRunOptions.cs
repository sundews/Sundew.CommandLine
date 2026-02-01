// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRunOptions.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Development.AcceptanceTests.Samples.KuandoBusylightForTeamCity;

using System;
using System.Collections.Generic;

public interface IRunOptions
{
    string HostName { get; }

    string BuildTypeId { get; }

    ICredentials? Credentials { get; }

    TimeSpan RefreshInterval { get; }

    IReadOnlyList<string> HidDeviceIds { get; }
}