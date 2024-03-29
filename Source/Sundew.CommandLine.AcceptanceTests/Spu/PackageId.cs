﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageId.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Spu;

using System;

public record PackageId(string Id, Version? Version = null);