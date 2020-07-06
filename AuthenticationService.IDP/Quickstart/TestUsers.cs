// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityServer4;

namespace IdentityServerHost.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };
                
                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "818727",
                        Username = "agent",
                        Password = "agent",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Agent Agent"),
                            new Claim(JwtClaimTypes.GivenName, "Agent"),
                            new Claim(JwtClaimTypes.FamilyName, "Agent"),
                            new Claim(JwtClaimTypes.Email, "agent@microservices-poc.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://localhost.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),
                            new Claim("role", "agent")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "88421113",
                        Username = "buyer",
                        Password = "buyer",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Buyer Buyer"),
                            new Claim(JwtClaimTypes.GivenName, "Buyer"),
                            new Claim(JwtClaimTypes.FamilyName, "Buyer"),
                            new Claim(JwtClaimTypes.Email, "buyer@microservices-poc.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://localhost.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),
                            new Claim("role", "user")
                        }
                    }
                };
            }
        }
    }
}