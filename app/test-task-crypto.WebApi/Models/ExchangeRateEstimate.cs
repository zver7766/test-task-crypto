namespace test_task_crypto.Models;

public class ExchangeRateEstimate
{
    public ExchangeRateEstimate(decimal outputAmount,
        string exchangeName)
    {
        OutputAmount = outputAmount;
        ExchangeName = exchangeName;
    }
    
    public decimal OutputAmount { get; init; }
    public string ExchangeName { get; init; }
    
    public override string ToString() =>
        $"{ExchangeName}: {OutputAmount}";
}