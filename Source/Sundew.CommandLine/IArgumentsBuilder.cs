// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IArgumentsBuilder.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Sundew.CommandLine.Internal;

    /// <summary>
    /// Interface for implementing an arguments builder.
    /// </summary>
    public interface IArgumentsBuilder
    {
        /// <summary>
        /// Gets or sets the default separators.
        /// </summary>
        Separators Separators { get; set; }

        /// <summary>
        /// Adds the required.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The helpText.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> the item is wrapped in double quotes.</param>
        /// <param name="separators">Defines the value separators.</param>
        void AddRequired(string name, string alias, Func<string> serialize, Action<string> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default);

        /// <summary>Adds the required.</summary>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        /// <param name="separators">The separators.</param>
        void AddRequired(string name, string alias, Func<CultureInfo, string> serialize, Action<string, CultureInfo> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default);

        /// <summary>
        /// Adds the required.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        /// <param name="separators">Defines the value separators.</param>
        void AddRequired(string name, string alias, Serialize serialize, Deserialize deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default);

        /// <summary>
        /// Adds the required.
        /// </summary>
        /// <typeparam name="TOptions">The type of the options.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="options">The options.</param>
        /// <param name="getDefault">The get default.</param>
        /// <param name="setOptions">The set options.</param>
        /// <param name="helpText">The help text.</param>
        void AddRequired<TOptions>(string name, string alias, TOptions? options, Func<TOptions> getDefault, Action<TOptions?> setOptions, string helpText)
            where TOptions : class, IArguments;

        /// <summary>
        /// Adds the required list.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="list">The list.</param>
        /// <param name="helpText">The helpText.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> each item is wrapped in double quotes.</param>
        void AddRequiredList(string name, string alias, IList<string> list, string helpText, bool useDoubleQuotes = false);

        /// <summary>
        /// Adds the required list.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="list">The list.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The helpText.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> each item is wrapped in double quotes.</param>
        void AddRequiredList<TValue>(string name, string alias, IList<TValue> list, Func<TValue, CultureInfo, string> serialize, Func<string, CultureInfo, TValue> deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>
        /// Adds the required list.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="list">The list.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddRequiredList<TValue>(string name, string alias, IList<TValue> list, Serialize<TValue> serialize, Deserialize<TValue> deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the optional.</summary>
        /// <param name="name">The short name.</param>
        /// <param name="alias">The long name.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The helpText.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> the item is wrapped in double quotes.</param>
        /// <param name="separators">Defines the value separators.</param>
        void AddOptional(string name, string alias, Func<string> serialize, Action<string> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default);

        /// <summary>Adds the optional.</summary>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        /// <param name="separators">The separators.</param>
        void AddOptional(string name, string alias, Func<CultureInfo, string> serialize, Action<string, CultureInfo> deserialize, string helpText, bool useDoubleQuotes = false, Separators separators = default);

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
        void AddOptional<TOptions>(string name, string alias, TOptions? options, Func<TOptions> getDefault, Action<TOptions?> setOptions, string helpText)
            where TOptions : class, IArguments;

        /// <summary>
        /// Adds the optional list.
        /// </summary>
        /// <param name="name">The short name.</param>
        /// <param name="alias">The long name.</param>
        /// <param name="list">The list.</param>
        /// <param name="helpText">The helpText.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> each item is wrapped in double quotes.</param>
        void AddOptionalList(string name, string alias, IList<string> list, string helpText, bool useDoubleQuotes = false);

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
        void AddOptionalList<TValue>(string name, string alias, IList<TValue> list, Func<TValue, CultureInfo, string> serialize, Func<string, CultureInfo, TValue> deserialize, string helpText, bool useDoubleQuotes = false);

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
        void AddOptionalList<TValue>(string name, string alias, IList<TValue> list, Serialize<TValue> serialize, Deserialize<TValue> deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>
        /// Adds the switch.
        /// </summary>
        /// <param name="name">The short name.</param>
        /// <param name="alias">The long name.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="setValue">The set value.</param>
        /// <param name="helpText">The helpText.</param>
        void AddSwitch(string name, string alias, bool value, Action<bool> setValue, string helpText);

        /// <summary>Adds the required value.</summary>
        /// <param name="name">The name.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddRequiredValue(string name, Func<string> serialize, Action<string> deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the required value.</summary>
        /// <param name="name">The name.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddRequiredValue(string name, Func<CultureInfo, string> serialize, Action<string, CultureInfo> deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the required value.</summary>
        /// <param name="name">The name.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddRequiredValue(string name, Serialize serialize, Deserialize deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the optional value.</summary>
        /// <param name="name">The name.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddOptionalValue(string name, Func<string> serialize, Action<string> deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the optional value.</summary>
        /// <param name="name">The name.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddOptionalValue(string name, Func<CultureInfo, string> serialize, Action<string, CultureInfo> deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the optional value.</summary>
        /// <param name="name">The name.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddOptionalValue(string name, Serialize serialize, Deserialize deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the required values.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddRequiredValues<TValue>(string name, IList<TValue> values, Func<TValue, CultureInfo, string> serialize, Func<string, CultureInfo, TValue> deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the required values.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddRequiredValues<TValue>(string name, IList<TValue> values, Serialize<TValue> serialize, Deserialize<TValue> deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the required values.</summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddRequiredValues(string name, IList<string> values, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the optional values.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddOptionalValues<TValue>(string name, IList<TValue> values, Func<TValue, CultureInfo, string> serialize, Func<string, CultureInfo, TValue> deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the optional values.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <param name="serialize">The serialize.</param>
        /// <param name="deserialize">The deserialize.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddOptionalValues<TValue>(string name, IList<TValue> values, Serialize<TValue> serialize, Deserialize<TValue> deserialize, string helpText, bool useDoubleQuotes = false);

        /// <summary>Adds the optional values.</summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="useDoubleQuotes">if set to <c>true</c> [use double quotes].</param>
        void AddOptionalValues(string name, IList<string> values, string helpText, bool useDoubleQuotes = false);
    }
}