using System.ComponentModel.DataAnnotations;

namespace Api.Model
{
    public class OrderBookEntryRequest
    {
        [Range(0.0001, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
