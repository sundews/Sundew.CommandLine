﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICredentials.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Samples.KuandoBusylightForTeamCity;

using System.Security;

public interface ICredentials
{
    string UserName { get; }

    SecureString Password { get; }
}