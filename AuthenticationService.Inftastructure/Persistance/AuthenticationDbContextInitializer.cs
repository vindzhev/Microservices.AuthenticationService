namespace AuthenticationService.Inftastructure.Persistance
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    
    using IdentityServer4;
    using IdentityServer4.Models;
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Mappers;
    
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;

    public static class AuthenticationDbContextInitializer
    {
        private static void InitializeTokenServer(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            //PersistedGrantDbContext migration
            using var persistantGrandContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            persistantGrandContext.Database.Migrate();

            //ConfigurationDbContext migration
            using var configurationContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            configurationContext.Database.Migrate();

            PopulateDatabase(app);
        }

        public static void PopulateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var configurationContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            //Seed clients
            bool frontEndClientExists = 
                configurationContext.Clients.Any(x => x.ClientId == "7ceea8f0-9ef6-4a41-b0d7-d4ebe99430bb");
            if (!frontEndClientExists)
            {
                Client frontEndClient = new Client
                {
                    ClientName = "Microservices.WebUI",
                    ClientId = "7ceea8f0-9ef6-4a41-b0d7-d4ebe99430bb",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    PostLogoutRedirectUris = new List<string>() { "http://localhost:4200" },
                    RedirectUris = new List<string>() { "http://localhost:4200/signin-oidc", "http://localhost:4200/redirect-silentrenew" },
                    AllowedScopes = new List<string>() 
                    { 
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile, 
                        "roles", "portal-gateway", "product-service-api" 
                    },
                    ClientSecrets = new List<Secret>() { new Secret("2af62f010fab4e558324b841d8006be1".Sha256()) },
                    Claims = new List<ClientClaim>()
                    {
                        new ClientClaim(string.Empty, "openid"),
                        new ClientClaim(string.Empty, "profile"),
                        new ClientClaim(string.Empty, "roles")
                    }
                };

                configurationContext.Clients.Add(frontEndClient.ToEntity());
            }

            //Seed identity resources
            IdentityResources.OpenId openId = new IdentityResources.OpenId();
            if (!configurationContext.IdentityResources.Any(x => x.Name == openId.Name))
                configurationContext.IdentityResources.Add(openId.ToEntity());

            IdentityResources.Profile profile = new IdentityResources.Profile();
            if (!configurationContext.IdentityResources.Any(x => x.Name == profile.Name))
                configurationContext.IdentityResources.Add(profile.ToEntity());

            IdentityResource role = new IdentityResource("roles", "Your role(s)", new[] { "role" });
            if (!configurationContext.IdentityResources.Any(x => x.Name == role.Name))
                configurationContext.IdentityResources.Add(role.ToEntity());

            //Seed api resources
            ApiResource portalGateway = new ApiResource("portal-gateway", "Portal.Gateway", new[] { "role" });
            if (!configurationContext.ApiResources.Any(x => x.Name == portalGateway.Name))
                configurationContext.ApiResources.Add(portalGateway.ToEntity());

            ApiResource productService = new ApiResource("product-service-api", "ProductService.API", new[] { "role" });
            if (!configurationContext.ApiResources.Any(x => x.Name == productService.Name))
                configurationContext.ApiResources.Add(productService.ToEntity());


            ApiResource pricingService = new ApiResource("pricing-service-api", "PricingService.API", new[] { "role" });
            if (!configurationContext.ApiResources.Any(x => x.Name == pricingService.Name))
                configurationContext.ApiResources.Add(pricingService.ToEntity());


            ApiResource policyService = new ApiResource("policy-service-api", "PolicyService.API", new[] { "role" });
            if (!configurationContext.ApiResources.Any(x => x.Name == policyService.Name))
                configurationContext.ApiResources.Add(policyService.ToEntity());


            ApiResource policySearchService = new ApiResource("policy-search-service-api", "PolicySearchService.API", new[] { "role" });
            if (!configurationContext.ApiResources.Any(x => x.Name == policySearchService.Name))
                configurationContext.ApiResources.Add(policySearchService.ToEntity());


            ApiResource paymentService = new ApiResource("payment-service-api", "PaymentService.API", new[] { "role" });
            if (!configurationContext.ApiResources.Any(x => x.Name == paymentService.Name))
                configurationContext.ApiResources.Add(paymentService.ToEntity());

            //.AddInMemoryApiScopes(new IdentityServer4.Models.ApiScope[] { new IdentityServer4.Models.ApiScope("portal-gateway", "Portal.Gateway", new[] { "role" })})
            ApiScope portalGatewayScope = new ApiScope("portal-gateway", "Portal.Gateway", new[] { "role" });
            if (!configurationContext.ApiScopes.Any(x => x.Name == portalGatewayScope.Name))
                configurationContext.ApiScopes.Add(portalGatewayScope.ToEntity());

            configurationContext.SaveChanges();
        }

        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<AuthenticationDbContext>();

            context.Database.EnsureCreated();

            InitializeTokenServer(app);

            context.SaveChanges();
        }
    }
}
