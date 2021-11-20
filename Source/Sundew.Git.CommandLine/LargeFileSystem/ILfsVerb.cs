// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILfsVerb.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Git.CommandLine.LargeFileSystem;

using Sundew.CommandLine;

/// <summary>Interface for limiting the verbs to be used with <see cref="Lfs"/>.</summary>
/// <seealso cref="Sundew.CommandLine.IVerb" />
public interface ILfsVerb : IVerb
{
}