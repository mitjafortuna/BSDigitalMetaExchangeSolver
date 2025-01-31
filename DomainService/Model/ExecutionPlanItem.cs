namespace DomainService.Model
{
    /// <summary>
    /// Item placeholder for execution plan result
    /// </summary>
    public class ExecutionPlanItem
    {
        /// <summary>
        /// Name of the exchange where the execution will take place
        /// </summary>
        public required string ExchangeName { get; set; }

        /// <summary>
        /// Price of the asset
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Amount of the asset
        /// </summary>
        public decimal Amount { get; set; }
    }
}
