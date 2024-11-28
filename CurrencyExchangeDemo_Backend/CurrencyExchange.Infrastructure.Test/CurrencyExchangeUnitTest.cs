using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CurrencyExchange.Infrastructure.Test
{
    public class CurrencyExchangeUnitTest
    {
        private readonly IConfiguration Configuration;

        public CurrencyExchangeUnitTest()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"RatesApiAddress", "https://api.fxratesapi.com/latest"}
            };

            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }

        [Fact]
        public async Task Test1Async()
        {

            var currencyExchange = new CurrencyExchange(Configuration);

            var rates = await currencyExchange.GetLatestRates();

            Assert.NotNull(rates);
            Assert.Equal(rates.Count(), 180);

        }
    }
}