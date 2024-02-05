using System.Reflection;
using FluentValidation;
using Kucoin.Net;
using Kucoin.Net.Objects;
using test_task_crypto.Parsers;
using test_task_crypto.Providers;
using test_task_crypto.Services;

namespace test_task_crypto.Extensions;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddBinanceServices(this IServiceCollection services)
    {
        services.AddScoped<IBinanceResponseParser, BinanceResponseParser>();
        services.AddScoped<IOrderPriceProvider, BinanceOrderPriceProvider>();
        
        return services;
    }
    
    public static IServiceCollection AddKuCoinServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderPriceProvider, KuCoinOrderPriceProvider>();

        // Use some secret manager to fill this API-Keys if needed
        services.AddKucoin(
            options => {
                options.ApiCredentials = new KucoinApiCredentials("API-KEY", "API-SECRET", "API-PASSPHRASE");
                options.RequestTimeout = TimeSpan.FromSeconds(60);
                options.FuturesOptions.ApiCredentials = new KucoinApiCredentials("FUTURES-API-KEY", "FUTURES-API-SECRET", "FUTURES-API-PASSPHRASE");
                options.FuturesOptions.AutoTimestamp = false;
            }); 
        
        return services;
    }
    
    public static IServiceCollection AddStandardServices(this IServiceCollection services, Assembly executingAssembly)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(executingAssembly));
        services.AddValidatorsFromAssemblyContaining(typeof(ServiceCollectionsExtensions));
        services.AddHttpClient();
        services.AddScoped<IPriceCalculator, PriceCalculator>();
        services.AddScoped<IExchangeRateAggregatorService, ExchangeRateAggregatorService>();

        return services;
    }
}