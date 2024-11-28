using CurrencyExchange.Application.Features.Queries.ViewModels;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace CurrencyExchange.Application
{
    public static class ServicesSetup
    {
        public static void AddCurrencyExchangeApplication(this IServiceCollection services)
        {
            RegisterApplicationMappings();
            services.RegisterMediatR();
            services.RegisterValidators();
        }

        public static void RegisterApplicationMappings()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(CurrencyRateViewModelMappingConfig).Assembly);
        }

        private static void RegisterMediatR(this IServiceCollection services)
        {
            services.AddMediatR(m =>
            {
                m.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
        }

        private static void RegisterValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            //services.AddValidatorsFromAssemblyContaining<AllRatesQueryValidator>();
        }
    }
}
