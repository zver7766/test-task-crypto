using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace test_task_crypto.Parsers;

public interface IBinanceResponseParser
{
    public decimal ParseFirstPrice(string recentTrade);
}

public class BinanceResponseParser : IBinanceResponseParser
{
    private readonly ILogger<BinanceResponseParser> _logger;

    public BinanceResponseParser(ILogger<BinanceResponseParser> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public decimal ParseFirstPrice(string recentTrade)
    {
        if (string.IsNullOrWhiteSpace(recentTrade))
        {
            _logger.LogError("Received an empty or null JSON string for recent trades.");
            throw new ArgumentException("Recent trade data cannot be null or empty.", nameof(recentTrade));
        }
        
        try
        {
            var jsonArray = JArray.Parse(recentTrade);

            if (jsonArray.Count > 0)
            {
                if (jsonArray[0]["price"] != null)
                {
                    return jsonArray[0].Value<decimal>("price");
                }
            }

            _logger.LogError("Error parsing 'price' from recent trade JSON.");
            throw new InvalidOperationException("Error parsing 'price' from recent trade JSON.");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error while parsing JSON response from Binance.");
            throw;
        }
    }
}