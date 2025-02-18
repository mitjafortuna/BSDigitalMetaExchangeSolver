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
    public void BuyOrder_ShouldSellSecondOrderOnExchangeABecauseExchangeBDoesNotHaveEnoughBalance()
    {
        var service = new ExchangeExecutionPlanService();

        var exchanges = new List<Exchange>
        {
            new Exchange
            {
                Name = "ExchangeA",
                Asks = new List<OrderBookEntry>()
                {
                    new() { Price = 3500m, Amount = 1m },
                },
                Bids = new List<OrderBookEntry>(),
                BalanceEUR = 3000m,
            },
            new Exchange
            {
                Name = "ExchangeB",
                Asks = new List<OrderBookEntry>
                {
                    new() { Price = 3000m, Amount = 0.2m },
                    new() { Price = 3000m, Amount = 0.3m },
                },
                Bids = new List<OrderBookEntry>(),
                BalanceEUR = 600m,
            },
        };

        var plan = service.GetBestExecutionPlan(exchanges, OrderType.Buy, 1);

        Assert.That(plan.Count, Is.EqualTo(2));

        Assert.That(plan[0].ExchangeName, Is.EqualTo("ExchangeB"));
        Assert.That(plan[1].ExchangeName, Is.EqualTo("ExchangeA"));
    }

    [Test]
    public void BuyOrder_ShouldSellSecondOrderOnExchangeBButOnlyPartiallyBecauseExchangeBDoesNotHaveEnoughBalance()
    {
        var service = new ExchangeExecutionPlanService();

        var exchanges = new List<Exchange>
        {
            new Exchange
            {
                Name = "ExchangeA",
                Asks = new List<OrderBookEntry>()
                {
                    new() { Price = 3500m, Amount = 1m },
                },
                Bids = new List<OrderBookEntry>(),
                BalanceEUR = 3000m,
            },
            new Exchange
            {
                Name = "ExchangeB",
                Asks = new List<OrderBookEntry>
                {
                    new() { Price = 3000m, Amount = 0.2m },
                    new() { Price = 3000m, Amount = 0.3m },
                },
                Bids = new List<OrderBookEntry>(),
                BalanceEUR = 900m,
            },
        };

        var plan = service.GetBestExecutionPlan(exchanges, OrderType.Buy, 1);

        Assert.That(plan.Count, Is.EqualTo(3));

        Assert.That(plan[0].ExchangeName, Is.EqualTo("ExchangeB"));
        Assert.That(plan[1].ExchangeName, Is.EqualTo("ExchangeB"));
        Assert.That(plan[1].Amount, Is.EqualTo(0.1m));
        Assert.That(plan[2].ExchangeName, Is.EqualTo("ExchangeA"));
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
    public void SellOrder_ShouldSellOnMultipleExchangesSinceThePricesExchangeDoesNotHaveEnoughBalance()
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

    [Test]
    public void SellOrder_ShouldSellOnAlsoOnExchangeBSinceExchangeADoesNotHaveEnoughBalanceToFulfillSecondBid()
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
                    new() { Price = 35500m, Amount = 0.5m },
                    new() { Price = 35400m, Amount = 0.5m },
                },
                BalanceBTC = 0.7m,
            },
            new Exchange
            {
                Name = "ExchangeB",
                Asks = new(),
                Bids = new List<OrderBookEntry>
                {
                    new() { Price = 35000m, Amount = 1m },
                },
                BalanceBTC = 1m,
            },
        };

        var plan = service.GetBestExecutionPlan(exchanges, OrderType.Sell, 1m);

        Assert.That(plan.Count, Is.EqualTo(3));

        Assert.That(plan[0].ExchangeName, Is.EqualTo("ExchangeA"));
        Assert.That(plan[0].Amount, Is.EqualTo(0.5m));
        Assert.That(plan[1].ExchangeName, Is.EqualTo("ExchangeA"));
        Assert.That(plan[1].Amount, Is.EqualTo(0.2m));
        Assert.That(plan[2].ExchangeName, Is.EqualTo("ExchangeB"));
        Assert.That(plan[2].Amount, Is.EqualTo(0.3m));
    }    
}
