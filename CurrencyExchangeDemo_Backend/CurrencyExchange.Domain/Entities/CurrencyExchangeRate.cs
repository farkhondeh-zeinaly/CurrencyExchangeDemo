namespace CurrencyExchange.Domain.Entities
{
    public class CurrencyExchangeRate
    {
        public string Base { get; set; }
        public string Target { get; set; }
        public decimal Rate { get; set; }
    }
}