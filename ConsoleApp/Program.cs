using DomainService;
using DomainService.Interface;
using DomainService.Model;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = CreateServices();
            var service = services.GetRequiredService<IExchangeExecutionPlanService>();
            try
            {
                service.GetBestExecutionPlan(
                    new List<Exchange>
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
                    },
                    OrderType.Buy,
                    20000m
                );
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
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
