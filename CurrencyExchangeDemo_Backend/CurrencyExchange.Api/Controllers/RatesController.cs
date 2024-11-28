using CurrencyExchange.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Api.Controllers
{

    [Route("api/v1/currency-exchange/[Controller]")]
    public class RatesController : BaseController
    {
        public RatesController(IMediator mediator, ILogger<BaseController> logger) : base(mediator, logger)
        {
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var command = new AllRatesQuery();

#if DEBUG
            //Tracing in debug mode
            Logger.LogTrace("Latest reates requested.");
#endif



            if (!ModelState.IsValid)
            {
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());
            }

            var result = await Mediator.Send(command);

            return Result(result);
        }
    }
}
