using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationExtensionMethods
{
    public static class PersistenceExtensionMethods
    {
        public static IServiceCollection AddPersitence(this IServiceCollection services, IConfiguration config)
        {
            //Database
            services.AddDbContext<FutureVestContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped(typeof(GenericRepository<>));
            services.AddScoped<CountryRepository>();
            services.AddScoped<MacroeconomicIndicatorRepository>();
            services.AddScoped<EconomicIndicatorRepository>();


            return services;
        }
    }
}
