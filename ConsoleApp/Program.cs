using System.Security.Cryptography;
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
            const decimal orderAmount = 10m;
            try
            {
                var buyResult = service
                    .GetBestExecutionPlanAsync(OrderType.Buy, orderAmount)
                    .Result;
                PrintResult(buyResult, orderAmount, OrderType.Buy);
                var sellResult = service
                    .GetBestExecutionPlanAsync(OrderType.Sell, orderAmount)
                    .Result;
                PrintResult(sellResult, orderAmount, OrderType.Sell);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private static void PrintResult(
            List<ExecutionPlanItem> result,
            decimal orderAmount,
            OrderType orderType
        )
        {
            Console.WriteLine($"Execution Plan for {orderAmount:N8} orders of {orderType}:");
            decimal totalSum = 0,
                totalAmount = 0;
            var exchangeColumnWidth = result.Max(item => item.ExchangeName.Length) + 2;
            var priceColumnWidth = result.Max(item => item.Price.ToString("N2").Length) + 2;
            var amountColumnWidth = result.Max(item => item.Amount.ToString("N8").Length) + 2;

            Console.WriteLine(
                "{0,-"
                    + exchangeColumnWidth
                    + "} {1,-"
                    + priceColumnWidth
                    + "} {2,-"
                    + amountColumnWidth
                    + "}",
                "Exchange",
                "Price",
                "Amount"
            );
            Console.WriteLine(
                new string('-', exchangeColumnWidth + priceColumnWidth + amountColumnWidth + 2)
            );
            foreach (var item in result)
            {
                totalSum += item.Amount * item.Price;
                totalAmount += item.Amount;
                Console.WriteLine(
                    "{0,-"
                        + exchangeColumnWidth
                        + "} {1,-"
                        + priceColumnWidth
                        + ":N2} {2,-"
                        + amountColumnWidth
                        + ":N8}",
                    item.ExchangeName,
                    item.Price,
                    item.Amount
                );
            }
            Console.WriteLine(
                new string('-', exchangeColumnWidth + priceColumnWidth + amountColumnWidth + 2)
            );
            Console.WriteLine(
                "{0,-"
                    + (exchangeColumnWidth - 5)
                    + "} avg: {1,-"
                    + priceColumnWidth
                    + ":N2} {2,-"
                    + amountColumnWidth
                    + ":N8}",
                "",
                totalSum / orderAmount,
                totalAmount
            );
            Console.WriteLine("Total SUM: {0:N2} EUR.", totalSum);
            Console.WriteLine();
        }

        private static ServiceProvider CreateServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IExchangesOrderBooksProvider, ExchangesOrderBooksProvider>()
                .AddSingleton<IExchangeExecutionPlanService, ExchangeExecutionPlanService>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
