using DomainService.Model;

namespace DomainService.Interface
{
    /// <summary>
    /// Inteface of a service that will calculate the execution plan for optimal exchanging of the asset
    /// </summary>
    public interface IExchangeExecutionPlanService
    {
        /// <summary>
        /// The function will calculate the best execution plan for the given exchanes.
        /// </summary>
        /// <param name="exchanges">Exchanges with order book lists</param>
        /// <param name="orderType">Type of order (Buy, Sell)</param>
        /// <param name="orderAmount">The amount of asset</param>
        /// <returns></returns>
        List<ExecutionPlanItem> GetBestExecutionPlan(
            List<Exchange> exchanges,
            OrderType orderType,
            decimal orderAmount
        );
    }
}
