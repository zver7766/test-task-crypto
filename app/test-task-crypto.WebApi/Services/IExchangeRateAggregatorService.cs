using test_task_crypto.Models;
using test_task_crypto.Providers;

namespace test_task_crypto.Services;

public interface IExchangeRateAggregatorService
{
    Task<IEnumerable<ExchangeRateInfo>> GetExchangeRatesInfoAsync(string baseCurrency, string quoteCurrency, CancellationToken cancellationToken);
}

public class ExchangeRateAggregatorService : IExchangeRateAggregatorService
{
    private readonly ILogger<ExchangeRateAggregatorService> _logger;
    private readonly IEnumerable<IOrderPriceProvider> _orderPriceProviders;

    public ExchangeRateAggregatorService(ILogger<ExchangeRateAggregatorService> logger,
        IEnumerable<IOrderPriceProvider> orderPriceProviders)
    {
        _logger = logger;
        _orderPriceProviders = orderPriceProviders;
    }

    public async Task<IEnumerable<ExchangeRateInfo>> GetExchangeRatesInfoAsync(string baseCurrency, string quoteCurrency, CancellationToken cancellationToken)
    {
        if (!_orderPriceProviders.Any())
        {
            _logger.LogWarning("No order price providers are configured.");
            return Enumerable.Empty<ExchangeRateInfo>();
        }
        
        baseCurrency = baseCurrency.ToUpper();
        quoteCurrency = quoteCurrency.ToUpper();
        
        var ratesTasks = _orderPriceProviders.Select(provider => FetchRateSafeAsync(provider, baseCurrency, quoteCurrency, cancellationToken));
        var rates = await Task.WhenAll(ratesTasks);
        return rates.Where(rate => rate != null).ToList()!;
    }
    
    private async Task<ExchangeRateInfo?> FetchRateSafeAsync(IOrderPriceProvider provider, string baseCurrency, string quoteCurrency, CancellationToken cancellationToken)
    {
        try
        {
            return await provider.GetLastOrderPriceAsync(baseCurrency, quoteCurrency, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch rate from {Provider} for {BaseCurrency} to {QuoteCurrency}.", provider.ExchangeName, baseCurrency, quoteCurrency);
            return null; // Or handle differently based on your error policy
        }
    }
}