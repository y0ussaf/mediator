// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer.Scopes;
using IdentityServer4;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        public static IEnumerable<ApiResource> ApiResources =>
           new List<ApiResource>()
           {
               new ApiResource("conversations")
               {
                   Name = "conversations",
                   Scopes =
                   {
                       ConversationsApiScopes.ConversationsRead,
                       ConversationsApiScopes.ConversationsWrite,
                       ConversationsApiScopes.ConversationsDelete
                   }
               }
           };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
              
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = {"https://localhost:3000/signin-oidc"},
                    FrontChannelLogoutUri = "https://localhost:3000/signout-oidc",
                    PostLogoutRedirectUris = {"https://localhost:3000/signout-callback-oidc"},
                    
                    RequireConsent = false,
                    
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        ConversationsApiScopes.ConversationsRead,
                        ConversationsApiScopes.ConversationsWrite, 
                        ConversationsApiScopes.ConversationsDelete
                    }
                },
            };
    }
}