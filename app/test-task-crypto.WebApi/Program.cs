using System.Reflection;
using test_task_crypto.Extensions;
using test_task_crypto.Middleware;

namespace test_task_crypto;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var executingAssembly = Assembly.GetExecutingAssembly();


        builder.Services
            .AddStandardServices(executingAssembly)
            .AddBinanceServices()
            .AddKuCoinServices();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.UseMiddleware<ExceptionMiddleware>();

        app.Run();
    }
}