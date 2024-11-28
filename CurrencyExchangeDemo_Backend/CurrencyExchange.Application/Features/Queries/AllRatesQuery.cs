using CurrencyExchange.Application.Abstraction;
using CurrencyExchange.Application.Features.Queries.ViewModels;
using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Resources;
using FluentResults;
using MediatR;
using Mapster;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Application.Features.Queries
{
    public class AllRatesQuery : IRequest<Result<IEnumerable<RateViewModel>>>
    {
        public class AllRatesQueryHandler : IRequestHandler<AllRatesQuery, Result<IEnumerable<RateViewModel>>>
        {
            private readonly ICurrencyExchange CurrencyExchange;
            private readonly ICachingService CachingService;
            private static Random _rnd = new Random();

            public AllRatesQueryHandler(ICurrencyExchange currencyExchange, ICachingService cachingService)
            {
                CurrencyExchange = currencyExchange;
                CachingService = cachingService;
            }

            public async Task<Result<IEnumerable<RateViewModel>>> Handle(AllRatesQuery request, CancellationToken cancellationToken)
            {
                var result = new Result<IEnumerable<RateViewModel>>();

                try
                {
                    var ratesViewModel = CachingService.GetData<IEnumerable<RateViewModel>>("Rates");

                    if (ratesViewModel is null)
                    {
                        var latestRates = await CurrencyExchange.GetLatestRates();

                        ratesViewModel = latestRates.Adapt<IEnumerable<RateViewModel>>();


                        CachingService.SetData(
                            "Rates",
                            ratesViewModel.Select(c => { c.Rate = c.Rate + (c.Rate * _rnd.Next(-15, +15) / 100); return c; }).ToList(),//Mock change in the rates just for demo
                            DateTime.Now.AddSeconds(10));
                    }


                    result.WithValue(ratesViewModel);
                    result.WithSuccess(Messages.OperationSucceeded);

                    return result;
                }
                catch (BusinessRuleValidationException ex)
                {
                    result.WithError(ex.Message);
                    return result.ToResult();
                }
                catch
                {
#if DEBUG
                    throw;
#endif

#if RELEASE
                    //log internal error
                    throw new Exception(Messages.InternalError);
#endif
                }
            }
        }

    }
}
