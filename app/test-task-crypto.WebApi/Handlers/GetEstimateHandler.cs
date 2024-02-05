using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using test_task_crypto.Extensions;
using test_task_crypto.Models;
using test_task_crypto.Queries;
using test_task_crypto.Services;

namespace test_task_crypto.Handlers;

[UsedImplicitly]
public class GetEstimateQueryHandler : IRequestHandler<GetEstimateQuery, ExchangeRateEstimate>
{
    private readonly ILogger<GetEstimateQueryHandler> _logger;
    private readonly IValidator<GetEstimateQuery> _validator;
    private readonly IExchangeRateAggregatorService _exchangeRateAggregatorService;

    public GetEstimateQueryHandler(ILogger<GetEstimateQueryHandler> logger,
        IValidator<GetEstimateQuery> validator,
        IExchangeRateAggregatorService exchangeRateAggregatorService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _exchangeRateAggregatorService = exchangeRateAggregatorService ?? throw new ArgumentNullException(nameof(exchangeRateAggregatorService));
    }
    
    public async Task<ExchangeRateEstimate> Handle(GetEstimateQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting rate estimation for {InputCurrency} to {OutputCurrency}, input amount: {InputAmount}", 
            request.InputCurrency, request.OutputCurrency, request.InputAmount);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            var rates = await _exchangeRateAggregatorService.GetExchangeRatesInfoAsync(
                request.InputCurrency,
                request.OutputCurrency,
                cancellationToken);

            var bestRate = rates.GetBestRate();
            var outputAmount = bestRate.CalculateOutputAmount(request.InputAmount);

            _logger.LogInformation("Best rate found: {Rate} from {ExchangeName} for {InputAmount} {InputCurrency}", 
                bestRate.Rate, bestRate.ExchangeName, request.InputAmount, request.InputCurrency);

            return new ExchangeRateEstimate(outputAmount, bestRate.ExchangeName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Error occurred while trying to get exchange rate estimates for {InputCurrency} to {OutputCurrency}, input amount: {InputAmount}",
                request.InputCurrency, request.OutputCurrency, request.InputAmount);
            throw;
        }
    }
}