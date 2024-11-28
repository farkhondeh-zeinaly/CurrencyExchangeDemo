using CurrencyExchange.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Application.Features.Queries.ViewModels
{
    public class CurrencyRateViewModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
              .NewConfig<CurrencyExchangeRate, RateViewModel>()
              .Map(dest => dest.Base, src => src.Base)
              .Map(dest => dest.Target, src => src.Target)
              .Map(dest => dest.Rate, src => src.Rate);

        }
    }
}
