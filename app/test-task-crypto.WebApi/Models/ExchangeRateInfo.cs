using Newtonsoft.Json;

namespace test_task_crypto.Models;

public class ExchangeRateInfo
{
    public ExchangeRateInfo(
        decimal rate,
        string exchangeName)
    {
        Rate = rate;
        ExchangeName = exchangeName;
    }

    [JsonProperty("price")]
    public decimal Rate { get; set; }
    public string ExchangeName { get; set; }
    
    public override string ToString() =>
        $"{ExchangeName}: {Rate}";
}