// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CredentialOptions.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Samples.KuandoBusylightForTeamCity
{
    public class CredentialOptions : IArguments, ICredentials
    {
        public CredentialOptions(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddRequired("u", "username", () => this.UserName, userName => this.UserName = userName, "Specifies the username");
            argumentsBuilder.AddRequired("p", "password", () => this.Password, password => this.Password = password, "Specifies the password");
        }
    }
}