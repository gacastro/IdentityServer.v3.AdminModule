/*
 * Copyright 2014, 2015 James Geall
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Management.Automation;
using System.Security.Claims;
using IdentityServer3.Core.Models;

namespace IdentityServer3.Admin.MongoDb.Powershell
{
    [Cmdlet(VerbsCommon.New, "Client")]
    public class CreateClient : PSCmdlet
    {
        [Parameter]
        public bool? Enabled { get; set; }
        [Parameter(Mandatory = true)]
        public string ClientId { get; set; }
        [Parameter]
        public Secret[] ClientSecrets { get; set; }

        [Parameter(Mandatory = true)]
        public string ClientName { get; set; }
        [Parameter]
        public string ClientUri { get; set; }
        [Parameter]
        public string LogoUri { get; set; }

        [Parameter]
        public bool? RequireConsent { get; set; }
        [Parameter]
        public bool? AllowRememberConsent { get; set; }
        [Parameter]
        public bool? EnableLocalLogin { get; set; }

        [Parameter]
        public Flows? Flow { get; set; }

        // in seconds
        [Range(0, Int32.MaxValue)]
        [Parameter]
        public int? IdentityTokenLifetime { get; set; }
        [Range(0, Int32.MaxValue)]
        [Parameter]
        public int? AccessTokenLifetime { get; set; }
        [Range(0, Int32.MaxValue)]
        [Parameter]
        public int? AuthorizationCodeLifetime { get; set; }

        [Range(0, Int32.MaxValue)]
        [Parameter]
        public int? AbsoluteRefreshTokenLifetime { get; set; }
        [Range(0, Int32.MaxValue)]
        [Parameter]
        public int? SlidingRefreshTokenLifetime { get; set; }
        [Parameter]
        public TokenUsage? RefreshTokenUsage { get; set; }
        [Parameter]
        public TokenExpiration? RefreshTokenExpiration { get; set; }

        [Parameter]
        public AccessTokenType? AccessTokenType { get; set; }

        [Parameter]
        public string[] IdentityProviderRestrictions { get; set; }
        [Parameter]
        public string[] PostLogoutRedirectUris { get; set; }
        [Parameter]
        public string[] RedirectUris { get; set; }
        [Parameter]
        public string[] AllowedScopes { get; set; }

        [Parameter]
        public bool? IncludeJwtId { get; set; }

        [Parameter]
        public bool? AlwaysSendClientClaims { get; set; }

        [Parameter]
        public bool? PrefixClientClaims { get; set; }

        [Parameter]
        public string[] AllowedCustomGrantTypes { get; set; }

        [Parameter]
        public Claim[] Claims { get; set; }

        [Parameter]
        public bool? AllowClientCredentialsOnly { get; set; }

        [Parameter]
        public bool? UpdateAccessTokenClaimsOnRefresh { get; set; }

        [Parameter]
        public string[] AllowedCorsOrigins { get; set; }

        [Parameter]
        public bool? AllowAccessToAllScopes { get; set; }

        [Parameter]
        public bool? AllowAccessToAllCustomGrantTypes { get; set; }

        [Parameter]
        public bool? AllowAccessTokensViaBrowser { get; set; }

        [Parameter]
        public bool? LogoutSessionRequired { get; set; }

        [Parameter]
        public bool? RequireSignOutPrompt { get; set; }

        [Parameter]
        public string LogoutUri { get; set; }

        protected override void ProcessRecord()
        {
            var client = new Client() { ClientId = ClientId, ClientName = ClientName };

            client.AbsoluteRefreshTokenLifetime =
                AbsoluteRefreshTokenLifetime.GetValueOrDefault(client.AbsoluteRefreshTokenLifetime);
            client.AccessTokenLifetime = AccessTokenLifetime.GetValueOrDefault(client.AccessTokenLifetime);
            client.AccessTokenType = AccessTokenType.GetValueOrDefault(client.AccessTokenType);
            client.EnableLocalLogin = EnableLocalLogin.GetValueOrDefault(client.EnableLocalLogin);
            client.AllowRememberConsent = AllowRememberConsent.GetValueOrDefault(client.AllowRememberConsent);
            client.AuthorizationCodeLifetime =
                AuthorizationCodeLifetime.GetValueOrDefault(client.AuthorizationCodeLifetime);

            client.ClientSecrets = (ClientSecrets ?? new Secret[] { }).ToList();
            client.ClientUri = ClientUri;
            client.Enabled = Enabled.GetValueOrDefault(client.Enabled);
            client.Flow = Flow.GetValueOrDefault(client.Flow);
            client.IdentityProviderRestrictions = (IdentityProviderRestrictions ?? client.IdentityProviderRestrictions.ToArray()).ToList();
            client.IdentityTokenLifetime = IdentityTokenLifetime.GetValueOrDefault(client.IdentityTokenLifetime);
            client.LogoUri = LogoUri;

            client.PostLogoutRedirectUris.AddRange(PostLogoutRedirectUris ?? new string[] { });
            client.RedirectUris.AddRange(RedirectUris ?? new string[] { });
            client.RefreshTokenExpiration = RefreshTokenExpiration.GetValueOrDefault(client.RefreshTokenExpiration);
            client.RefreshTokenUsage = RefreshTokenUsage.GetValueOrDefault(client.RefreshTokenUsage);
            client.RequireConsent = RequireConsent.GetValueOrDefault(client.RequireConsent);
            client.AllowedScopes.AddRange(AllowedScopes ?? new string[] { });
            client.SlidingRefreshTokenLifetime =
                SlidingRefreshTokenLifetime.GetValueOrDefault(client.SlidingRefreshTokenLifetime);
            client.IncludeJwtId = IncludeJwtId.GetValueOrDefault(client.IncludeJwtId);
            client.AlwaysSendClientClaims = AlwaysSendClientClaims.GetValueOrDefault(client.AlwaysSendClientClaims);
            client.PrefixClientClaims = PrefixClientClaims.GetValueOrDefault(client.PrefixClientClaims);
            client.Claims.AddRange((Claims ?? new Claim[] { }));
            client.AllowedCustomGrantTypes.AddRange((AllowedCustomGrantTypes ?? new string[] { }));
            client.AllowClientCredentialsOnly =
                AllowClientCredentialsOnly.GetValueOrDefault(client.AllowClientCredentialsOnly);
            client.AllowedCorsOrigins.AddRange(AllowedCorsOrigins ?? new String[] { });
            client.UpdateAccessTokenClaimsOnRefresh =
                UpdateAccessTokenClaimsOnRefresh.GetValueOrDefault(client.UpdateAccessTokenClaimsOnRefresh);
            client.AllowAccessToAllScopes = AllowAccessToAllScopes.GetValueOrDefault(client.AllowAccessToAllScopes);
            client.AllowAccessToAllCustomGrantTypes = AllowAccessToAllCustomGrantTypes.GetValueOrDefault(client.AllowAccessToAllCustomGrantTypes);
            client.AllowAccessTokensViaBrowser = AllowAccessTokensViaBrowser.GetValueOrDefault(client.AllowAccessTokensViaBrowser);
            client.LogoutSessionRequired = LogoutSessionRequired.GetValueOrDefault(client.LogoutSessionRequired);
            client.RequireSignOutPrompt = RequireSignOutPrompt.GetValueOrDefault(client.RequireSignOutPrompt);
            client.LogoutUri = LogoutUri;
            WriteObject(client);
        }
    }
}