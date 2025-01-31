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
                var exchanges = GetExchanges();
                var result = service.GetBestExecutionPlan(exchanges, OrderType.Buy, 0.3m);
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

        private static List<Exchange> GetExchanges()
        {
            return new List<Exchange>
            {
                new Exchange
                {
                    Name = "ExchangeA",
                    Bids = new List<OrderBookEntry>
                    {
                        new() { Price = 99000m, Amount = 1m },
                    },
                    Asks = new List<OrderBookEntry>
                    {
                        new() { Price = 99100m, Amount = 1m },
                    },
                    BalanceBTC = 3m,
                    BalanceEUR = 2000000m,
                },
            };
        }

        private static ServiceProvider CreateServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IExchangeExecutionPlanService>(new ExchangeExecutionPlanService())
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
