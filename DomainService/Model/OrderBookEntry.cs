namespace DomainService.Model
{
    /// <summary>
    /// Record of an order book
    /// </summary>
    public class OrderBookEntry
    {
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
