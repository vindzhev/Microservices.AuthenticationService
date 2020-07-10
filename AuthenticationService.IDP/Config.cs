// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthenticationService.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "Your role(s)", new [] { "role" })
            };

        public static IEnumerable<ApiResource> ApiResources => 
            new ApiResource[] 
            {
                new ApiResource("portal-gateway", "Portal.Gateway", new[] { "role" }),
                new ApiResource("product-service-api", "ProductService.API", new[] { "role" }),
                new ApiResource("pricing-service-api", "PricingService.API", new[] { "role" }),
                new ApiResource("policy-service-api", "PolicyService.API", new[] { "role" }),
                new ApiResource("policy-search-service-api", "PolicySearchService.API", new[] { "role" }),
                new ApiResource("payment-service-api", "PaymentService.API", new[] { "role" }),
            };

        public static IEnumerable<Client> Clients =>
            new Client[] 
            { 
                new Client
                {
                    
                    ClientName = "Microservices.WebUI",
                    ClientId = "7ceea8f0-9ef6-4a41-b0d7-d4ebe99430bb",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    PostLogoutRedirectUris = new List<string>() { "http://localhost:4200" },
                    RedirectUris = new List<string>() { "http://localhost:4200/signin-oidc", "http://localhost:4200/redirect-silentrenew" },
                    AllowedScopes = new List<string>() { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "roles", "portal-gateway", "product-service-api" },
                    ClientSecrets = new List<Secret>() { new Secret("2af62f010fab4e558324b841d8006be1".Sha256()) }
                }
            };
    }
}