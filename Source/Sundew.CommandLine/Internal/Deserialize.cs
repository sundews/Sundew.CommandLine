// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Deserialize.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

using System;
using System.Globalization;

/// <summary>Delegate for deserializing from a <see cref="ReadOnlySpan{Char}"/>.</summary>
/// <param name="value">The value.</param>
/// <param name="ci">The culture info.</param>
public delegate void Deserialize(ReadOnlySpan<char> value, CultureInfo ci);
