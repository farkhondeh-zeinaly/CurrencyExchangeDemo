using CurrencyExchange.Application;
using CurrencyExchange.Infrastructure;

namespace CurrencyExchange.Api
{
    public static class ServicesSetup
    {
        public static void AddCurrencyExchangeDependencies(this IServiceCollection services)
        {
            //Add infrastructures
            services.AddCurrencyExchangeInfrastructure();

            //Add application layer dependencies
            services.AddCurrencyExchangeApplication();

        }
    }
}
