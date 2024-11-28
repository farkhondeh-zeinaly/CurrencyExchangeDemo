using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Api.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// MediatR object
        /// </summary>
        protected IMediator Mediator { get; }
        public readonly ILogger<BaseController> Logger;

        protected BaseController(IMediator mediator, ILogger<BaseController> logger)
        {
            Mediator = mediator;
            Logger = logger;
        }

        /// <summary>
        /// A convention to return Ok(http response code: 200) if the result is successful otherwise BadResult( http response code: 400) 
        /// </summary>
        /// <typeparam name="T">Data to return</typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        protected ActionResult Result<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.ToCleanResult());
            }
            else
            {
                return BadRequest(result.ToResult().ToCleanResult());
            }
        }

        /// <summary>
        /// A convention to return Ok(http response code: 200) if the result is successful otherwise BadResult( http response code: 400) 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected ActionResult Result(Result result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.ToCleanResult());
            }
            else
            {
                return BadRequest(result.ToCleanResult());
            }
        }

        /// <summary>
        /// A convention to return BadResult( http response code: 400) with given error messages  
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        protected ActionResult BadResult(params string[] messages)
        {
            return Result(new Result().WithErrors(messages));
        }
    }
}
