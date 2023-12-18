﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageId.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Spt;

using System.Text.RegularExpressions;
using NuGet.Versioning;

public record PackageId(string Id, string? VersionPattern = null);

public record PackageIdAndVersion(string Id, NuGetVersion NuGetVersion);

public record PackageUpdate(string Id, NuGetVersion NuGetVersion, NuGetVersion UpdatedNuGetVersion) : PackageIdAndVersion(Id, NuGetVersion);

public record VersionMatcher(Regex Regex, string Pattern);