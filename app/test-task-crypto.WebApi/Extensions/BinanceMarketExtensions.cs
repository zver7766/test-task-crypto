using Binance.Spot;

namespace test_task_crypto.Extensions;

public static class BinanceMarketExtensions
{
    // Consider to using a More Descriptive Return Type, not just string
    public static async Task<string> GetRecentTradeAsync(this Market market, string symbol, int limit = 1)
    {
        try
        {
            return await market.RecentTradesList(symbol, limit);
        }
        catch (Exception)
        {
            // Returning empty string, not the best solution, but suitable in this implementation
            return string.Empty;
        }
    }
}