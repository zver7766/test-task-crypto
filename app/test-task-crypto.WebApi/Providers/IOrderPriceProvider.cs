using JetBrains.Annotations;
using test_task_crypto.Models;

namespace test_task_crypto.Providers;

public interface IOrderPriceProvider
{
    [UsedImplicitly]
    string ExchangeName { get; }
    Task<ExchangeRateInfo> GetLastOrderPriceAsync(string baseCurrency, string quoteCurrency, CancellationToken cancellationToken);
}