using DomainService;
using DomainService.Model;

namespace UnitTests;

[TestFixture]
public class ExchangeExecutionPlanServiceTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void BuyOrder_ShouldChooseCheapestExchange()
    {
        var service = new ExchangeExecutionPlanService();

        var exchanges = new List<Exchange>
        {
            new Exchange
            {
                Name = "ExchangeA",
                Asks = new List<OrderBookEntry>
                {
                    new() { Price = 35000m, Amount = 1m },
                },
                Bids = new(),
                BalanceEUR = 20000m,
            },
            new Exchange
            {
                Name = "ExchangeB",
                Asks = new List<OrderBookEntry>
                {
                    new() { Price = 35500m, Amount = 1m },
                },
                Bids = new(),
                BalanceEUR = 20000m,
            },
        };

        var plan = service.GetBestExecutionPlan(exchanges, OrderType.Buy, 0.5m);

        Assert.That(plan.Count, Is.EqualTo(1));
        Assert.That(plan[0].ExchangeName, Is.EqualTo("ExchangeA"));
    }

    [Test]
    public void BuyOrder_ShouldGetFromMultipleExchangesSinceCheapestExchangeDoesNotHaveEnoughBalance()
    {
        var service = new ExchangeExecutionPlanService();

        var exchanges = new List<Exchange>
        {
            new Exchange
            {
                Name = "ExchangeA",
                Asks = new List<OrderBookEntry>
                {
                    new() { Price = 35000m, Amount = 1m },
                },
                Bids = new(),
                BalanceEUR = 1000m,
            },
            new Exchange
            {
                Name = "ExchangeB",
                Asks = new List<OrderBookEntry>
                {
                    new() { Price = 35500m, Amount = 1m },
                },
                Bids = new(),
                BalanceEUR = 20000m,
            },
        };

        var plan = service.GetBestExecutionPlan(exchanges, OrderType.Buy, 0.5m);

        Assert.That(plan.Count, Is.EqualTo(2));
        Assert.That(plan[0].ExchangeName, Is.EqualTo("ExchangeA"));
        Assert.That(plan[1].ExchangeName, Is.EqualTo("ExchangeB"));
    }

    [Test]
    public void SellOrder_ShouldChooseHighestBid()
    {
        var service = new ExchangeExecutionPlanService();

        var exchanges = new List<Exchange>
        {
            new Exchange
            {
                Name = "ExchangeA",
                Asks = new(),
                Bids = new List<OrderBookEntry>
                {
                    new() { Price = 35000m, Amount = 1m },
                },
                BalanceBTC = 1m,
            },
            new Exchange
            {
                Name = "ExchangeB",
                Asks = new(),
                Bids = new List<OrderBookEntry>
                {
                    new() { Price = 35500m, Amount = 1m },
                },
                BalanceBTC = 1m,
            },
        };

        var plan = service.GetBestExecutionPlan(exchanges, OrderType.Sell, 0.5m);

        Assert.That(plan.Count, Is.EqualTo(1));
        Assert.That(plan[0].ExchangeName, Is.EqualTo("ExchangeB"));
    }

    [Test]
    public void SellOrder_ShouldSellOnMultipleExchangesSinceThePriciesExchangeDoesNotHaveEnoughBalance()
    {
        var service = new ExchangeExecutionPlanService();

        var exchanges = new List<Exchange>
        {
            new Exchange
            {
                Name = "ExchangeA",
                Asks = new(),
                Bids = new List<OrderBookEntry>
                {
                    new() { Price = 35000m, Amount = 1m },
                },
                BalanceBTC = 1m,
            },
            new Exchange
            {
                Name = "ExchangeB",
                Asks = new(),
                Bids = new List<OrderBookEntry>
                {
                    new() { Price = 35500m, Amount = 1m },
                },
                BalanceBTC = 0.3m,
            },
        };

        var plan = service.GetBestExecutionPlan(exchanges, OrderType.Sell, 0.5m);

        Assert.That(plan.Count, Is.EqualTo(2));

        Assert.That(plan[0].ExchangeName, Is.EqualTo("ExchangeB"));
        Assert.That(plan[1].ExchangeName, Is.EqualTo("ExchangeA"));
    }
}
