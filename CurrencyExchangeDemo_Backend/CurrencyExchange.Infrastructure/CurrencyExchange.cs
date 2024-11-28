using CurrencyExchange.Application.Abstraction;
using CurrencyExchange.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using CurrencyExchange.Application;

namespace CurrencyExchange.Infrastructure
{
    public class CurrencyExchange : ICurrencyExchange
    {
        private readonly IConfiguration Configuration;


        public CurrencyExchange(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IEnumerable<CurrencyExchangeRate>> GetLatestRates()
        {
            //return new CurrencyExchangeRate[] {
            //    new CurrencyExchangeRate(){ Base="USD",Target="ADA" ,Rate=1},
            //    new CurrencyExchangeRate(){ Base="USD",Target="AED" ,Rate=1},
            //    new CurrencyExchangeRate(){ Base="USD",Target="AFN" ,Rate=1},
            //    new CurrencyExchangeRate(){ Base="USD",Target="ALL" ,Rate=1},
            //    new CurrencyExchangeRate(){ Base="USD",Target="AMD" ,Rate=1}
            //};


            var apiAddress = Configuration["RatesApiAddress"];

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(SmsBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            // Add an Accept header for JSON format.  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(new Uri(apiAddress)).Result;

            if (response.IsSuccessStatusCode)
            {
                var dataJsonString = await response.Content.ReadAsStringAsync();

                // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)
                dataJsonString =
                    dataJsonString
                    .TrimStart('\"')
                    .TrimEnd('\"')
                    .Replace("\\", "")
                    .Replace("\\", "");

                var rates = JsonConvert.DeserializeObject<ApiRates>(dataJsonString);



                var exchangeRates = rates.Rates.Select(r =>
                    new CurrencyExchangeRate()
                    {
                        Base = rates.Base,
                        Rate = r.Value,
                        Target = r.Key
                    }).AsEnumerable();

                return exchangeRates;
                //return JsonSerializer.Deserialize<SmsSendResult>(dataJsonString);
            }
            else
            {
                throw new BusinessRuleValidationException($"Error [{response.StatusCode}] in fetching rates: {response.Content}");
            }


        }
    }

    public class ApiRates
    {
        public string Base { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}