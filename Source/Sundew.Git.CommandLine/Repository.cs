// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Git.CommandLine
{
    /// <summary>Represents a git name.</summary>
    public class Repository
    {
        /// <summary>Initializes a new instance of the <see cref="Repository"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="refspec">The refspec.</param>
        public Repository(string name, Refspec refspec)
        {
            this.Name = name;
            this.Refspec = refspec;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the refspec.
        /// </summary>
        public Refspec Refspec { get; }

        /// <summary>Initializes a new instance of the <see cref="Repository"/> class.</summary>
        /// <param name="repository">The repository.</param>
        /// <returns>The parse repository.</returns>
        public static Repository Parse(string repository)
        {
            var array = repository.Split(' ');
            return new Repository(array[0], new Refspec(array[1]));
        }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{this.Name} {this.Refspec}";
        }
    }
}