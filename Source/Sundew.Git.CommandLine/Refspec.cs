// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Refspec.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Git.CommandLine;

/// <summary>
/// Represents a git refspec.
/// </summary>
public class Refspec
{
    private readonly string refspec;

    /// <summary>Initializes a new instance of the <see cref="Refspec"/> class.</summary>
    /// <param name="refspec">The refspec.</param>
    public Refspec(string refspec)
    {
        this.refspec = refspec;
    }

    /// <summary>Converts to string.</summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString()
    {
        return this.refspec;
    }
}