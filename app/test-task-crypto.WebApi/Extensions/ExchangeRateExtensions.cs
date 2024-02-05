using test_task_crypto.Models;

namespace test_task_crypto.Extensions;

public static class ExchangeRateExtensions
{
    public static ExchangeRateInfo GetBestRate(this IEnumerable<ExchangeRateInfo> exchangeRates)
    {
        if (exchangeRates == null)
        {
            throw new ArgumentNullException(nameof(exchangeRates), "The collection cannot be null.");
        }
        
        var bestRate = exchangeRates.MaxBy(r => r.Rate);

        if (bestRate == null)
        {
            throw new ArgumentException("Cannot find the best rate from an empty collection.", nameof(exchangeRates));
        }

        return bestRate;
    }
    
    public static decimal CalculateOutputAmount(this ExchangeRateInfo exchangeRate, decimal inputAmount)
    {
        if (exchangeRate == null)
        {
            throw new ArgumentNullException(nameof(exchangeRate), "Best rate cannot be null.");
        }

        return inputAmount * exchangeRate.Rate;
    }
}