namespace test_task_crypto.Services;

public interface IPriceCalculator
{
    decimal InvertPrice(decimal price);
}

public class PriceCalculator : IPriceCalculator
{
    private readonly ILogger<PriceCalculator> _logger;

    public PriceCalculator(ILogger<PriceCalculator> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public decimal InvertPrice(decimal price)
    {
        // Example threshold - adjust based on domain-specific knowledge
        const decimal minimumValidPrice = 0.0001m;
        
        if (price == 0)
        {
            _logger.LogError("Attempted to invert a price of 0, which is not valid.");
            throw new InvalidOperationException("Cannot invert a price of 0.");
        }
        if (price < 0)
        {
            _logger.LogError("Attempted to invert a negative price of {Price}, which is not valid.", price);
            throw new InvalidOperationException("Cannot invert a negative price.");
        } 
        if (Math.Abs(price) < minimumValidPrice)
        {
            _logger.LogWarning("Attempted to invert a price very close to 0 ({Price}), which may lead to precision issues.", price);
            // Depending on requirements, this could also throw an exception
        }
        
        return 1 / price;
    }
}