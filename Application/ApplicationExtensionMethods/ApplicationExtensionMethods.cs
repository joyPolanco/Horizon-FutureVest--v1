using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationExtensionMethods
{
    public static class ApplicationExtensionMethods
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Services
            services.AddScoped<CountryService>();
            services.AddScoped<MacroindicatorService>();
            services.AddScoped<EconomicIndicatorService>();
            services.AddScoped<EstimatedRateConfigurationService>();
            services.AddScoped<RankingService>();
            services.AddScoped<SimulationRankingService>();
            services.AddScoped<SimulationMacroindicatorService>();

            return services;
        }
    }
}
