using DomainService;
using DomainService.Model;

namespace UnitTests;

public class ExchangeExecutionPlanServiceTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void TestGetBestExecutionPlanThrowsNotImplemented()
    {
        var service = new ExchangeExecutionPlanService();
        Assert.That(
            () =>
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
            },
            Throws.InstanceOf<NotImplementedException>()
        );
    }
}
