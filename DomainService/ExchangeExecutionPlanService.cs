using DomainService.Interface;
using DomainService.Model;

namespace DomainService;

public class ExchangeExecutionPlanService : IExchangeExecutionPlanService
{
    public List<ExecutionPlanItem> GetBestExecutionPlan(
        List<Exchange> exchanges,
        OrderType orderType,
        decimal orderAmount
    )
    {
        var executionPlan = new List<ExecutionPlanItem>();
        decimal remainingAmount = orderAmount;

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
                    var maxAffordable = Math.Min(balanceEUR / price, amount);
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
                    var tradeAmount = Math.Min(remainingAmount, Math.Min(amount, balanceBTC));

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
