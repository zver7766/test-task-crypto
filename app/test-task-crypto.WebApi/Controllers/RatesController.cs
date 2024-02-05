using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using test_task_crypto.Models;
using test_task_crypto.Queries;

namespace test_task_crypto.Controllers;

[Route("api/rates")]
[ApiController]
[Produces("application/json")]
public class RatesController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public RatesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Retrieves the exchange rate information for multiple exchanges between the specified base and quote currencies.
    /// </summary>
    /// <param name="baseCurrency">The base currency code (e.g., BTC) for which to retrieve exchange rate information.</param>
    /// <param name="quoteCurrency">The quote currency code (e.g., USDT) against which the base currency is measured.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IActionResult"/> that, when executed, will produce an HTTP response with the exchange rate information.</returns>
    /// <response code="200">Returns the exchange rate information for multiple exchanges and the specified currency pair.</response>
    /// <response code="400">If the query parameters are not valid, a bad request response is returned.</response>
    [HttpGet]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ExchangeRateInfo>))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRates([FromQuery] string baseCurrency, [FromQuery] string quoteCurrency, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRatesQuery(baseCurrency, quoteCurrency), cancellationToken);
        return Ok(result);
    }
}