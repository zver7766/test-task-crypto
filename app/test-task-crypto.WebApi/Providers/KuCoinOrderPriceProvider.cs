using Kucoin.Net.Interfaces.Clients;
using test_task_crypto.Extensions;
using test_task_crypto.Models;
using test_task_crypto.Services;

namespace test_task_crypto.Providers;

public class KuCoinOrderPriceProvider : IOrderPriceProvider
{
    private readonly ILogger<KuCoinOrderPriceProvider> _logger;
    private readonly IKucoinRestClient _client;
    private readonly IPriceCalculator _priceCalculator;

    public KuCoinOrderPriceProvider(ILogger<KuCoinOrderPriceProvider> logger,
        IKucoinRestClient client,
        IPriceCalculator priceCalculator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _priceCalculator = priceCalculator ?? throw new ArgumentNullException(nameof(priceCalculator));
    }

    public string ExchangeName => "KuCoin";

    public async Task<ExchangeRateInfo> GetLastOrderPriceAsync(string baseCurrency, string quoteCurrency, CancellationToken cancellationToken)
    {
        var symbol = $"{baseCurrency}-{quoteCurrency}";
        try
        {
            var lastTrade = await _client.GetLastTradeAsync(symbol, cancellationToken);
            if (lastTrade is null)
            {
                _logger.LogInformation("Trade for symbol: {symbol} is empty for {exchangeName}, reversing it", symbol, ExchangeName);
                
                var reversedSymbol = $"{quoteCurrency}-{baseCurrency}";
                lastTrade = await _client.GetLastTradeAsync(reversedSymbol, cancellationToken);
                
                if (lastTrade is null)
                {
                    _logger.LogError("No trade history found for symbol: {symbol} for {exchangeName}", symbol, ExchangeName);
                    throw new InvalidOperationException($"No trade history found for symbol: {reversedSymbol}, baseCurrency: {baseCurrency}, quoteCurrency: {quoteCurrency} for {ExchangeName}");
                }
                
                lastTrade.Price = _priceCalculator.InvertPrice(lastTrade.Price);
            }

            return new ExchangeRateInfo(lastTrade.Price, ExchangeName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching trade history from {ExchangeName}", ExchangeName);
            throw;
        }
    }
}