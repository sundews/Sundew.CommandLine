// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Serialize{TValue}.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.Internal;

using System;
using System.Globalization;

/// <summary>Delegate for serializing a value into a <see cref="ReadOnlySpan{Char}"/>.</summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
/// <param name="value">The value.</param>
/// <param name="ci">The culture info.</param>
/// <returns>The serialized value.</returns>
public delegate ReadOnlySpan<char> Serialize<in TValue>(TValue value, CultureInfo ci);
