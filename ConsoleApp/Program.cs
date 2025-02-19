using DomainService;
using DomainService.Interface;
using DomainService.Model;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = CreateServices();
            var service = services.GetRequiredService<IExchangeExecutionPlanService>();
            try
            {
                var result = service.GetBestExecutionPlanAsync(OrderType.Buy, 3m).Result;
                PrintResult(result);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private static void PrintResult(List<ExecutionPlanItem> result)
        {
            Console.WriteLine("Execution Plan:");
            foreach (var item in result)
            {
                Console.WriteLine(
                    $"Exchange: {item.ExchangeName}, Price: {item.Price}, Amount: {item.Amount}"
                );
            }
        }

        private static ServiceProvider CreateServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IExchangeExecutionPlanService>(
                    new ExchangeExecutionPlanService(new ExchangesOrderBooksProvider())
                )
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
