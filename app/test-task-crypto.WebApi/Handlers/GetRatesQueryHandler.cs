using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using test_task_crypto.Models;
using test_task_crypto.Queries;
using test_task_crypto.Services;

namespace test_task_crypto.Handlers;

[UsedImplicitly]
public class GetRatesQueryHandler : IRequestHandler<GetRatesQuery, IEnumerable<ExchangeRateInfo>>
{
    private readonly ILogger<GetRatesQueryHandler> _logger;
    private readonly IValidator<GetRatesQuery> _validator;
    private readonly IExchangeRateAggregatorService _exchangeRateAggregatorService;

    public GetRatesQueryHandler(ILogger<GetRatesQueryHandler> logger,
        IValidator<GetRatesQuery> validator,
        IExchangeRateAggregatorService exchangeRateAggregatorService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _exchangeRateAggregatorService = exchangeRateAggregatorService ?? throw new ArgumentNullException(nameof(exchangeRateAggregatorService));
    }

    public async Task<IEnumerable<ExchangeRateInfo>> Handle(GetRatesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetRatesQuery for BaseCurrency: {BaseCurrency}, QuoteCurrency: {QuoteCurrency}",
            request.BaseCurrency, request.QuoteCurrency);
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            var result = await _exchangeRateAggregatorService.GetExchangeRatesInfoAsync(
                request.BaseCurrency,
                request.QuoteCurrency,
                cancellationToken);

            _logger.LogInformation("Successfully retrieved rates for BaseCurrency: {BaseCurrency}, QuoteCurrency: {QuoteCurrency}",
                request.BaseCurrency, request.QuoteCurrency);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve rates for BaseCurrency: {BaseCurrency}, QuoteCurrency: {QuoteCurrency}",
                request.BaseCurrency, request.QuoteCurrency);
            throw; // Consider how you want to handle exceptions. Rethrowing keeps the error propagation behavior.
        }
    }
}