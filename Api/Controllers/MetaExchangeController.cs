using Api.Model;
using DomainService.Interface;
using DomainService.Model;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/metaexchange")]
public class MetaExchangeController : ControllerBase
{
    private readonly IExchangeExecutionPlanService _exchangeExecutionPlanService;

    public MetaExchangeController(IExchangeExecutionPlanService exchangeExecutionPlanService)
    {
        _exchangeExecutionPlanService = exchangeExecutionPlanService;
    }

    [HttpPost("execution-plan")]
    public ActionResult<List<(string exchange, decimal price, decimal amount)>> GetExecutionPlan(
        [FromBody] ExecutionRequest request
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var plan = _exchangeExecutionPlanService.GetBestExecutionPlan(
            ConvertExchangesRequestToExchanges(request.Exchanges),
            Enum.Parse<OrderType>(request.OrderType),
            request.OrderAmount
        );
        return Ok(plan);
    }

    private List<Exchange> ConvertExchangesRequestToExchanges(
        List<ExchangeRequest> exchangeRequests
    )
    {
        return exchangeRequests
            .Select(er => new Exchange()
            {
                Name = er.Name,
                BalanceBTC = er.BalanceBTC,
                BalanceEUR = er.BalanceEUR,
                Asks = er
                    .Asks.Select(a => new OrderBookEntry() { Price = a.Price, Amount = a.Amount })
                    .ToList(),
                Bids = er
                    .Bids.Select(b => new OrderBookEntry() { Price = b.Price, Amount = b.Amount })
                    .ToList(),
            })
            .ToList();
    }
}
