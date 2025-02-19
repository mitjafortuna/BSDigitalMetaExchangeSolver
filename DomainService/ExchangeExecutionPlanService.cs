using DomainService.Interface;
using DomainService.Model;

namespace DomainService;

public class ExchangeExecutionPlanService : IExchangeExecutionPlanService
{
    private readonly IExchangesOrderBooksProvider _orderBooksProvider;
    private List<Exchange>? _exchanges = null;

    private async Task<List<Exchange>> GetExchanges()
    {
        if (_exchanges == null)
        {
            _exchanges = await _orderBooksProvider.GetExchangesAsync();
        }

        return _exchanges;
    }

    public ExchangeExecutionPlanService(IExchangesOrderBooksProvider orderBooksProvider)
    {
        _orderBooksProvider = orderBooksProvider;
    }

    public async Task<List<ExecutionPlanItem>> GetBestExecutionPlanAsync(
        OrderType orderType,
        decimal orderAmount
    )
    {
        var executionPlan = new List<ExecutionPlanItem>();
        decimal remainingAmount = orderAmount;
        var exchanges = await GetExchanges();
        switch (orderType)
        {
            case OrderType.Buy:
                var sortedAsks = exchanges
                    .SelectMany(e => e.Asks.Select(a => (e.Name, a.Price, a.Amount, e.BalanceEUR)))
                    .Where(e => e.BalanceEUR > 0)
                    .OrderBy(e => e.Price)
                    .ToList();

                foreach (var (exchangeName, price, amount, balanceEUR) in sortedAsks)
                {
                    var unusedBalanceEUR = GetUnusedBalanceEUR(
                        balanceEUR,
                        exchangeName,
                        executionPlan
                    );
                    var maxAffordable = Math.Min(unusedBalanceEUR / price, amount);
                    var tradeAmount = Math.Min(remainingAmount, maxAffordable);
                    if (tradeAmount <= 0)
                        continue;

                    AddToExecutionPlan(executionPlan, exchangeName, price, tradeAmount);

                    remainingAmount -= tradeAmount;

                    if (remainingAmount <= 0)
                        break;
                }
                break;
            case OrderType.Sell:
                var sortedBids = exchanges
                    .SelectMany(e => e.Bids.Select(b => (e.Name, b.Price, b.Amount, e.BalanceBTC)))
                    .Where(e => e.BalanceBTC > 0)
                    .OrderByDescending(e => e.Price)
                    .ToList();

                foreach (var (exchangeName, price, amount, balanceBTC) in sortedBids)
                {
                    var unusedBalanceBTC = GetUnusedBalanceBTC(
                        balanceBTC,
                        exchangeName,
                        executionPlan
                    );
                    var tradeAmount = Math.Min(remainingAmount, Math.Min(amount, unusedBalanceBTC));

                    if (tradeAmount <= 0)
                        continue;

                    AddToExecutionPlan(executionPlan, exchangeName, price, tradeAmount);

                    remainingAmount -= tradeAmount;

                    if (remainingAmount <= 0)
                        break;
                }
                break;
        }

        return executionPlan;
    }

    private decimal GetUnusedBalanceBTC(
        decimal startBalanceBTC,
        string exchangeName,
        List<ExecutionPlanItem> executionPlan
    )
    {
        return startBalanceBTC
            - executionPlan.Where(e => e.ExchangeName == exchangeName).Sum(e => e.Amount);
    }

    private decimal GetUnusedBalanceEUR(
        decimal startBalanceEUR,
        string exchangeName,
        List<ExecutionPlanItem> executionPlan
    )
    {
        return startBalanceEUR
            - executionPlan.Where(e => e.ExchangeName == exchangeName).Sum(e => e.Price * e.Amount);
    }

    private static void AddToExecutionPlan(
        List<ExecutionPlanItem> executionPlan,
        string exchangeName,
        decimal price,
        decimal tradeAmount
    )
    {
        executionPlan.Add(
            new ExecutionPlanItem()
            {
                ExchangeName = exchangeName,
                Price = price,
                Amount = tradeAmount,
            }
        );
    }
}
