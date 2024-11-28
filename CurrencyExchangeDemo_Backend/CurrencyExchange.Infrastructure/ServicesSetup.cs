using CurrencyExchange.Application.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Infrastructure
{
    public static class ServicesSetup
    {
        public static void AddCurrencyExchangeInfrastructure(this IServiceCollection services)
        {
            services
                .AddScoped<ICurrencyExchange, CurrencyExchange>()
                .AddScoped<ICachingService, CachingService>()
                ;
        }
    }
}
