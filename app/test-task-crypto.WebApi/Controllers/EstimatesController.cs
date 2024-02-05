using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using test_task_crypto.Models;
using test_task_crypto.Queries;

namespace test_task_crypto.Controllers;

[Route("api/estimates")]
[ApiController]
[Produces("application/json")]
public class EstimatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EstimatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves the best exchange rate estimate for converting a specified amount from one currency to another.
    /// </summary>
    /// <param name="inputCurrency">The currency code of the input amount (e.g., "USDT").</param>
    /// <param name="outputCurrency">The target currency code for conversion (e.g., "BTC").</param>
    /// <param name="inputAmount">The amount of the input currency to be converted.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>An <see cref="IActionResult"/> that, when executed, will produce an HTTP response containing the exchange rate estimate.</returns>
    /// <response code="200">Returns the exchange rate estimate, including the exchange name and the output amount after conversion.</response>
    /// <response code="400">If the query parameters are not valid, indicating a bad request.</response>
    [HttpGet]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ExchangeRateEstimate))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEstimate([FromQuery] string inputCurrency, [FromQuery] string outputCurrency, [FromQuery] decimal inputAmount, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEstimateQuery(inputCurrency, outputCurrency, inputAmount), cancellationToken);
        return Ok(result);
    }
}