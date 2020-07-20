// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AuthenticationService.Inftastructure;
using AuthenticationService.Inftastructure.Persistance;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace AuthenticationService.IDP
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            services.AddInfrastructure(Configuration);

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(setupAction =>
                    setupAction.ConfigureDbContext =
                        (builder) => builder.UseNpgsql(Configuration.GetConnectionString("AuthenticationServiceConnection"),
                        (options) => options.MigrationsAssembly(typeof(AuthenticationDbContext).Assembly.FullName)))
                .AddOperationalStore(setupAction =>
                    setupAction.ConfigureDbContext =
                        (builder) => builder.UseNpgsql(Configuration.GetConnectionString("AuthenticationServiceConnection"),
                        (options) => options.MigrationsAssembly(typeof(AuthenticationDbContext).Assembly.FullName)));

            services.AddCors();

            // not recommended for production - you need to store your key material somewhere secure
            //builder.AddDeveloperSigningCredential();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
                IdentityModelEventSource.ShowPII = true;

                app.InitializeDatabase();
            }

            app.UseCors(x => x.WithOrigins(new string[] { "http://localhost:4200" }).AllowAnyHeader().AllowAnyMethod());

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
