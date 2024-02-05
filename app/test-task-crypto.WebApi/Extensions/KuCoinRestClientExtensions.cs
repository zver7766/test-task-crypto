using Kucoin.Net.Interfaces.Clients;
using Kucoin.Net.Objects.Models.Spot;

namespace test_task_crypto.Extensions;

public static class KuCoinRestClientExtensions
{
    // Can consider about Null Checks and Error Handling, but it this iteration functional is sufficient
    public static async Task<KucoinTrade?> GetLastTradeAsync(this IKucoinRestClient client, string symbol, CancellationToken cancellationToken)
    {
        var tradeHistoryData = await client.SpotApi.ExchangeData.GetTradeHistoryAsync(symbol, cancellationToken);
        var lastTrade = tradeHistoryData.Data.LastOrDefault();

        return lastTrade;
    }
}