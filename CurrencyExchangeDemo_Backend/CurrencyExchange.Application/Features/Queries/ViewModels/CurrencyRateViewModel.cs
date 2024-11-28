using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Application.Features.Queries.ViewModels
{
    public class RateViewModel
    {
        public RateViewModel(string @base, string target, decimal rate)
        {
            Base = @base;
            Target = target;
            Rate = rate;
        }

        public string Base { get; set; }
        public string Target { get; set; }

        public decimal Rate { get; set; }
    }
}
