// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IArgumentMissingInfo.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine;

using System.Text;

internal interface IArgumentMissingInfo
{
    void AppendMissingArgumentsHint(StringBuilder stringBuilder);
}