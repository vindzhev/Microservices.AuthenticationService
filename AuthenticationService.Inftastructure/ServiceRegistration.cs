using AuthenticationService.Inftastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Inftastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
               .AddDbContext<AuthenticationDbContext>(options =>
                   options.UseNpgsql(configuration.GetConnectionString("AuthenticationServiceConnection"),
                   x => x.MigrationsAssembly(typeof(AuthenticationDbContext).Assembly.FullName)));

            return services;
        }
    }
}
