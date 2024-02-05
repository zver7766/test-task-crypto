using Binance.Spot;
using test_task_crypto.Extensions;
using test_task_crypto.Models;
using test_task_crypto.Parsers;
using test_task_crypto.Services;

namespace test_task_crypto.Providers;

public class BinanceOrderPriceProvider : IOrderPriceProvider
{
    private readonly ILogger<BinanceOrderPriceProvider> _logger;
    private readonly HttpClient _httpClient;
    private readonly IBinanceResponseParser _responseParser;
    private readonly IPriceCalculator _priceCalculator;

    public BinanceOrderPriceProvider(ILogger<BinanceOrderPriceProvider> logger,
        HttpClient httpClient,
        IBinanceResponseParser responseParser,
        IPriceCalculator priceCalculator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _responseParser = responseParser ?? throw new ArgumentNullException(nameof(responseParser));
        _priceCalculator = priceCalculator ?? throw new ArgumentNullException(nameof(priceCalculator));
    }

    public string ExchangeName => "Binance";

    public async Task<ExchangeRateInfo> GetLastOrderPriceAsync(string baseCurrency, string quoteCurrency, CancellationToken cancellationToken)
    {
        try
        {
            var (recentTrade, isReversed) = await FetchAndDetermineSymbolOrientationAsync(baseCurrency, quoteCurrency);
            var firstPrice = _responseParser.ParseFirstPrice(recentTrade);

            if (isReversed)
            {
                firstPrice = _priceCalculator.InvertPrice(firstPrice);
            }

            return new ExchangeRateInfo(firstPrice, ExchangeName);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request to get recent trades for {ExchangeName} failed.", ExchangeName);
            throw;
        }
    }
    
    private async Task<(string recentTrade, bool isReversed)> FetchAndDetermineSymbolOrientationAsync(string baseCurrency, string quoteCurrency)
    {
        var symbol = $"{baseCurrency}{quoteCurrency}";
        var marketService = new Market(_httpClient);
        var recentTrade = await marketService.GetRecentTradeAsync(symbol);
        var isSymbolReversed = false;

        if (string.IsNullOrEmpty(recentTrade))
        {
            _logger.LogInformation("Trade for symbol: {Symbol} is empty for {ExchangeName}, reversing it.", symbol, ExchangeName);
            
            var reversedSymbol = $"{quoteCurrency}{baseCurrency}";
            recentTrade = await marketService.GetRecentTradeAsync(reversedSymbol);

            if (!string.IsNullOrEmpty(recentTrade))
            {
                isSymbolReversed = true;
            }
            else
            {
                _logger.LogError("No trade history found for symbol: {Symbol} or its reverse for {ExchangeName}.", symbol, ExchangeName);
                throw new InvalidOperationException($"No trade history found for symbol: {symbol} or its reverse, for {ExchangeName}.");
            }
        }

        return (recentTrade, isSymbolReversed);
    }
}