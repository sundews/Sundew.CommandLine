// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChoiceBuilder.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System;
using System.Collections.Generic;
using System.Globalization;
using Sundew.CommandLine.Internal;

/// <summary>
/// Interface for implementing a choice builder.
/// </summary>
public interface IChoiceBuilder
{
    /// <summary>
    /// Adds the optional.
    /// </summary>
    /// <param name="name">The short name.</param>
    /// <param name="alias">The long name.</param>
    /// <param name="serialize">The serialize.</param>
    /// <param name="deserialize">The deserialize.</param>
    /// <param name="helpText">The helpText.</param>
    /// <param name="useDoubleQuotes">if set to <c>true</c> the item is wrapped in double quotes.</param>
    /// <param name="separators">Defines the value separators.</param>
    /// <param name="defaultValueText">The default value help text.</param>
    /// <returns>This instance.</returns>
    IChoiceBuilder Add(string? name, string alias, Func<string?> serialize, Action<string> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default, string? defaultValueText = null);

    /// <summary>
    /// Adds the optional.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="alias">The alias.</param>
    /// <param name="serialize">The serialize.</param>
    /// <param name="deserialize">The deserialize.</param>
    /// <param name="helpText">The help text.</param>
    /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
    /// <param name="separators">The separators.</param>
    /// <param name="defaultValueText">The default value help text.</param>
    /// <returns>This instance.</returns>
    IChoiceBuilder Add(string? name, string alias, Func<CultureInfo, string?> serialize, Action<string, CultureInfo> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default, string? defaultValueText = null);

    /// <summary>
    /// Adds the optional.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="name">The name.</param>
    /// <param name="alias">The alias.</param>
    /// <param name="options">The options.</param>
    /// <param name="getDefault">The get default.</param>
    /// <param name="setOptions">The set options.</param>
    /// <param name="helpText">The help text.</param>
    /// <param name="defaultValueText">The default value help text.</param>
    /// <returns>This instance.</returns>
    IChoiceBuilder Add<TOptions>(string? name, string alias, TOptions? options, Func<TOptions> getDefault, Action<TOptions> setOptions, string helpText, string? defaultValueText = null)
        where TOptions : class, IArguments;

    /// <summary>
    /// Adds the optional enum.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    /// <param name="name">The name.</param>
    /// <param name="alias">The alias.</param>
    /// <param name="getEnumFunc">The get enum function.</param>
    /// <param name="setEnumAction">The set enum action.</param>
    /// <param name="helpText">The help text.</param>
    /// <param name="separators">The separators.</param>
    /// <param name="defaultValueText">The default value text.</param>
    IChoiceBuilder AddEnum<TEnum>(string? name, string alias, Func<TEnum> getEnumFunc, Action<TEnum> setEnumAction, string helpText, Separators separators = default, string? defaultValueText = null)
        where TEnum : Enum;

    /// <summary>
    /// Adds the optional enum.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    /// <param name="name">The name.</param>
    /// <param name="alias">The alias.</param>
    /// <param name="getEnumFunc">The get enum function.</param>
    /// <param name="setEnumAction">The set enum action.</param>
    /// <param name="options">The options.</param>
    /// <param name="helpText">The help text.</param>
    /// <param name="separators">The separators.</param>
    /// <param name="defaultValueText">The default value text.</param>
    IChoiceBuilder AddEnum<TEnum>(string? name, string alias, Func<TEnum> getEnumFunc, Action<TEnum> setEnumAction, IEnumerable<TEnum> options, string helpText, Separators separators = default, string? defaultValueText = null)
        where TEnum : Enum;

    /// <summary>
    /// Adds the optional list.
    /// </summary>
    /// <param name="name">The short name.</param>
    /// <param name="alias">The long name.</param>
    /// <param name="list">The list.</param>
    /// <param name="helpText">The helpText.</param>
    /// <param name="useDoubleQuotes">if set to <c>true</c> each item is wrapped in double quotes.</param>
    /// <param name="defaultValueText">The default value help text.</param>
    /// <returns>This instance.</returns>
    IChoiceBuilder AddList(string? name, string alias, IList<string> list, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null);

    /// <summary>
    /// Adds the optional list.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="name">The name.</param>
    /// <param name="alias">The alias.</param>
    /// <param name="list">The list.</param>
    /// <param name="serialize">The serialize.</param>
    /// <param name="deserialize">The deserialize.</param>
    /// <param name="helpText">The help text.</param>
    /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
    /// <param name="defaultValueText">The default value help text.</param>
    /// <returns>This instance.</returns>
    IChoiceBuilder AddList<TValue>(string? name, string alias, IList<TValue> list, Func<TValue, CultureInfo, string> serialize, Func<string, CultureInfo, TValue> deserialize, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null);

    /// <summary>
    /// Adds the optional list.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="name">The short name.</param>
    /// <param name="alias">The long name.</param>
    /// <param name="list">The list.</param>
    /// <param name="serialize">The serialize.</param>
    /// <param name="deserialize">The deserialize.</param>
    /// <param name="helpText">The helpText.</param>
    /// <param name="useDoubleQuotes">if set to <c>true</c> each item is wrapped in double quotes.</param>
    /// <param name="defaultValueText">The default value help text.</param>
    /// <returns>This instance.</returns>
    IChoiceBuilder AddList<TValue>(string? name, string alias, IList<TValue> list, Serialize<TValue> serialize, Deserialize<TValue> deserialize, string helpText, bool useDoubleQuotes = false, string? defaultValueText = null);

    /// <summary>
    /// Adds the switch.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="alias">The alias.</param>
    /// <param name="value">if set to <c>true</c> [value].</param>
    /// <param name="setValue">The set value.</param>
    /// <param name="helpText">The help text.</param>
    /// <returns>This instance.</returns>
    IChoiceBuilder AddSwitch(string? name, string alias, bool value, Action<bool> setValue, string helpText);
}