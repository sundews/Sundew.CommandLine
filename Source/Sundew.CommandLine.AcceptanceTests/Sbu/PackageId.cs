// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageId.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Sbu
{
    using System;

    public record PackageId(string Id, Version? Version = null);
}