using DomainService.Model;

namespace DomainService.Interface
{
    /// <summary>
    /// Interface of a service that will calculate the execution plan for optimal exchanging of the asset
    /// </summary>
    public interface IExchangeExecutionPlanService
    {
        /// <summary>
        /// The function will calculate the best execution plan for the exchanges that are defined int the Resources/DomainService/Resources/OrderBooks.json.
        /// </summary>
        /// <param name="orderType">Type of order (Buy, Sell)</param>
        /// <param name="orderAmount">The amount of asset</param>
        /// <returns></returns>
        Task<List<ExecutionPlanItem>> GetBestExecutionPlanAsync(
            OrderType orderType,
            decimal orderAmount
        );
    }
}
