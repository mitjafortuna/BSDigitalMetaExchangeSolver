namespace DomainService.Model
{
    /// <summary>
    /// Exchange data
    /// </summary>
    public class Exchange
    {
        /// <summary>
        /// Name of the exchange
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// All the bids for the exchange
        /// </summary>
        public required List<OrderBookEntry> Bids { get; set; }

        /// <summary>
        /// All the asks for the exchange
        /// </summary>
        public required List<OrderBookEntry> Asks { get; set; }

        /// <summary>
        /// Current BTC balance of the exchange
        /// </summary>
        public decimal BalanceBTC { get; set; }

        /// <summary>
        /// Current EUR balance of the exchange
        /// </summary>
        public decimal BalanceEUR { get; set; }
    }
}
