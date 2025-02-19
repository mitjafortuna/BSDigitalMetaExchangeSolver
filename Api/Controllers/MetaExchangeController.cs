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
    public async Task<
        ActionResult<List<(string exchange, decimal price, decimal amount)>>
    > GetExecutionPlan([FromBody] ExecutionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var plan = await _exchangeExecutionPlanService.GetBestExecutionPlanAsync(
            Enum.Parse<OrderType>(request.OrderType),
            request.OrderAmount
        );
        return Ok(plan);
    }
}
