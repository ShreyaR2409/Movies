using App.Core.Interfaces;
using Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
         IConfigurationManager configuration)
        {
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddDbContext<AppDbContext>((provider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });

            return services;
        }
    }
}
