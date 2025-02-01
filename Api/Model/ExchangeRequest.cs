using System.ComponentModel.DataAnnotations;

namespace Api.Model
{
    public class ExchangeRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "Exchange name cannot be empty.")]
        public required string Name { get; set; }

        [Required]
        public List<OrderBookEntryRequest> Bids { get; set; } = new();

        [Required]
        public List<OrderBookEntryRequest> Asks { get; set; } = new();

        [Range(0, double.MaxValue, ErrorMessage = "BalanceBTC cannot be negative.")]
        public decimal BalanceBTC { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "BalanceEUR cannot be negative.")]
        public decimal BalanceEUR { get; set; }
    }
}
