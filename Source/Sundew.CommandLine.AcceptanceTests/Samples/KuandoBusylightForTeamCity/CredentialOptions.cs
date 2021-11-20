// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CredentialOptions.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Samples.KuandoBusylightForTeamCity;

using System.Net;
using System.Security;

public class CredentialOptions : IArguments, ICredentials
{
    public CredentialOptions()
        : this(default!, default!)
    {
    }

    public CredentialOptions(string userName, SecureString password)
    {
        this.UserName = userName;
        this.Password = password;
    }

    public string UserName { get; private set; }

    public SecureString Password { get; private set; }

    public string HelpText { get; } = "The credentials";

    public void Configure(IArgumentsBuilder argumentsBuilder)
    {
        argumentsBuilder.AddRequired("u", "username", () => this.UserName, userName => this.UserName = userName, "Specifies the username");
        argumentsBuilder.AddRequired("p", "password", () => FromSecureString(this.Password), password => this.Password = ToSecureString(password), "Specifies the password");
    }

    private static string FromSecureString(SecureString password)
    {
        return new NetworkCredential(string.Empty, password).Password;
    }

    private static SecureString ToSecureString(string password)
    {
        var secureString = new SecureString();
        foreach (var character in password)
        {
            secureString.AppendChar(character);
        }

        return secureString;
    }
}