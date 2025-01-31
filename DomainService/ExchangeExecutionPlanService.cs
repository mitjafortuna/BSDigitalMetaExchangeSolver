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
        throw new NotImplementedException();
    }
}
